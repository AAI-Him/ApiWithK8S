using AAISAPClient.SapRfcFunctions;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient
{
    abstract class BaseSQSWorker(
        ILogger logger,
        IServiceProvider serviceProvider,
        IAmazonSQS sqsClient)
        : BackgroundService
    {
        protected AWSSQSChannelOption? _AWSSQSChannelOption = null;
        protected Dictionary<string, int> _RetryMessage = new Dictionary<string, int>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_AWSSQSChannelOption == null)
            {
                throw new ArgumentNullException($"{nameof(_AWSSQSChannelOption)} does not have configuration for {AAISAPConstants.ZRFC135_CREATE_COSTPLAN_V4}");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                while (true)
                {
                    try
                    {
                        var receiveMessageRequest = new ReceiveMessageRequest
                        {
                            QueueUrl = _AWSSQSChannelOption.QueueUrl,
                            MaxNumberOfMessages = _AWSSQSChannelOption.MaxNumberOfMessages,
                            WaitTimeSeconds = 5000,
                            MessageAttributeNames = new List<string> { "All" }
                        };

                        using (var scope = serviceProvider.CreateScope())
                        {
                            var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

                            if (receiveMessageResponse.Messages.Count > 0)
                            {
                                //using (IRfcClient client = serviceProvider.GetRequiredService<IRfcClient>())
                                foreach (var message in receiveMessageResponse.Messages)
                                {
                                    // TODO: define action to SAP
                                    Console.WriteLine($"Deserialized messgae: {JsonConvert.SerializeObject(message)}");

                                    bool retval = await ProcessMessage(message);

                                    if (retval)
                                    {
                                        await DeleteMessage(_AWSSQSChannelOption, message.ReceiptHandle);
                                        Console.WriteLine($"{message.ReceiptHandle} has been removed from queue.");
                                    }
                                    else
                                    {
                                        if (!_RetryMessage.ContainsKey(message.MessageId))
                                        {
                                            _RetryMessage.Add(message.MessageId, 0);
                                        }

                                        if (_RetryMessage.TryGetValue(message.MessageId, out int retryCount) && retryCount < _AWSSQSChannelOption.MaxRetryCount)
                                        {
                                            // retry same message payload for retry count < 3
                                            _RetryMessage[message.MessageId]++;
                                            continue;
                                        }
                                        else
                                        {
                                            // move to DLQ and dequeue
                                            await sqsClient.SendMessageAsync(_AWSSQSChannelOption.DLQueueUrl, message.Body);
                                            await DeleteMessage(_AWSSQSChannelOption, message.ReceiptHandle);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // handling of invalid endpoint-url will be here
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        protected virtual async Task<bool> ProcessMessage(Message message)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("");
            });
            return true;
        }

        protected virtual async Task DeleteMessage(AWSSQSChannelOption option, string receiptHandle)
        {
            if (sqsClient == null)
                throw new ArgumentNullException($"{nameof(sqsClient)} cannot be empty.");
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = option.QueueUrl,
                    ReceiptHandle = receiptHandle
                };

                await sqsClient.DeleteMessageAsync(deleteMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting message: {ex.Message}");
                throw;
            }
        }
    }
}

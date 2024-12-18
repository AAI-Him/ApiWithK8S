using AAISAPClient.SapRfcFunctions;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SapCo2.Abstract;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace AAISAPClient
{
    public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IAmazonSQS sqsClient) 
        : BackgroundService
    {
        private string _QueueUrl = "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/smartzone-queue";
        private string _DLQueueUrl = "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/smartzone-dlq";
        private int _MaxRetryCount = 3;

        private Dictionary<string, int> _RetryMessage = new Dictionary<string, int>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
                            QueueUrl = _QueueUrl,
                            MaxNumberOfMessages = 1,
                            WaitTimeSeconds = 5000,
                            MessageAttributeNames = new List<string> { "All" }
                        };

                        var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

                        if (receiveMessageResponse.Messages.Count > 0)
                        {
                            foreach (var message in receiveMessageResponse.Messages)
                            {
                                // TODO: define action to SAP
                                Console.WriteLine($"Deserialized messgae: {JsonConvert.SerializeObject(message)}");

                                bool retval = await ProcessMessage(message);

                                if (retval)
                                {
                                    await DeleteMessage(message.ReceiptHandle);
                                    Console.WriteLine($"{message.ReceiptHandle} has been removed from queue.");
                                }
                                else
                                {
                                    if (!_RetryMessage.ContainsKey(message.MessageId))
                                    {
                                        _RetryMessage.Add(message.MessageId, 0);
                                    }

                                    if (_RetryMessage.TryGetValue(message.MessageId, out int retryCount) && retryCount < _MaxRetryCount)
                                    {
                                        // retry same message payload for retry count < 3
                                        _RetryMessage[message.MessageId]++;
                                        continue;
                                    }
                                    else
                                    {
                                        // move to DLQ and dequeue
                                        await sqsClient.SendMessageAsync(_DLQueueUrl, message.Body);
                                        await DeleteMessage(message.ReceiptHandle);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(50000, stoppingToken);
                    }
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task<bool> ProcessMessage(Message message)
        {
            //using (IRfcClient client = serviceProvider.GetRequiredService<IRfcClient>())
            //{

            //}
            await Task.Run(() => Console.WriteLine("Hello world") );
            return false;
        }

        private async Task DeleteMessage(string receiptHandle)
        {
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = _QueueUrl,
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

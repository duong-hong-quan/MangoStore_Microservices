using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerUserQueue;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private readonly string orderCreated_Topic;
        private readonly string orderCreated_Email_Subcription;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _userProcessor;
        private ServiceBusProcessor _emailOrderPlaceProcessor;




        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");

            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subcription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subcription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _userProcessor = client.CreateProcessor(registerUserQueue);
            _emailOrderPlaceProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subcription);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _userProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _userProcessor.ProcessErrorAsync += ErrorHandler;
            await _userProcessor.StartProcessingAsync();

            _emailOrderPlaceProcessor.ProcessMessageAsync += OnOrderPlaceRequestReceived;
            _emailOrderPlaceProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlaceProcessor.StartProcessingAsync();
        }

        private async Task OnOrderPlaceRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
               await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body);
            try {
                _emailService.EmailCartAndLog(cartDto);
                await args.CompleteMessageAsync(args.Message);

            }catch(Exception ex) {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
           await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _userProcessor.StopProcessingAsync();
            await _userProcessor.DisposeAsync();

            await _emailOrderPlaceProcessor.StopProcessingAsync();
            await _emailOrderPlaceProcessor.DisposeAsync();
        }
    }
}

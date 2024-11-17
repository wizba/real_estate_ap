using real_estate_api.ErrorHandling;
using real_estate_api.Services;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Person>> GetAllUsersAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task<Person> GetUserByIdAsync(long id)
        {
            // Remove the try-catch here since we want exceptions to propagate
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                throw new NotFoundException($"User with ID {id} not found");
            }
            return client;
        }


        public async Task AddUserAsync(Person client)
        {
            try
            {
                if (client == null)
                {
                    throw new BadRequestException("Client cannot be null");
                }

                // Validate client properties
                

                // Check if user already exists
                var existingClient = await _clientRepository.GetByIdAsync(client.Id);
                if (existingClient != null)
                {
                    throw new BadRequestException($"Client with ID {client.Id} already exists");
                }

                await _clientRepository.AddAsync((Client)client);
            }
            catch (InvalidCastException ex)
            {
                _logger.LogError(ex, "Invalid cast while adding client {ClientId}", client?.Id);
                throw new BadRequestException("Invalid client data format");
            }
            catch (Exception ex) when (ex is not BadRequestException)
            {
                _logger.LogError(ex, "Error occurred while adding client {ClientId}", client?.Id);
                throw new Exception("An error occurred while adding the client", ex);
            }
        }

        public async Task UpdateUserAsync(Person client)
        {
            try
            {
                if (client == null)
                {
                    throw new BadRequestException("Client cannot be null");
                }

                // Validate client properties
                //ValidateClient(client);

                // Check if user exists
                var existingClient = await _clientRepository.GetByIdAsync(client.Id);
                if (existingClient == null)
                {
                    throw new NotFoundException($"Client with ID {client.Id} not found");
                }

                await _clientRepository.UpdateAsync((Client)client);
            }
            catch (InvalidCastException ex)
            {
                _logger.LogError(ex, "Invalid cast while updating client {ClientId}", client?.Id);
                throw new BadRequestException("Invalid client data format");
            }
            catch (Exception ex) when (ex is not BadRequestException && ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error occurred while updating client {ClientId}", client?.Id);
                throw new Exception("An error occurred while updating the client", ex);
            }
        }

        public async Task DeleteUserAsync(long id)
        {
            try
            {
                // Check if user exists before deletion
                var existingClient = await _clientRepository.GetByIdAsync(id);
                if (existingClient == null)
                {
                    throw new NotFoundException($"Client with ID {id} not found");
                }

                await _clientRepository.DeleteAsync(id);
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error occurred while deleting client {ClientId}", id);
                throw new Exception("An error occurred while deleting the client", ex);
            }
        }


        private void ValidateClient(Person client)
        {
            var validationErrors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(client.FirstName))
            {
                validationErrors.Add("name", new[] { "Name is required" });
            }

            if (string.IsNullOrWhiteSpace(client.Email))
            {
                validationErrors.Add("email", new[] { "Email is required" });
            }
            else if (!IsValidEmail(client.Email))
            {
                validationErrors.Add("email", new[] { "Invalid email format" });
            }

            // Add more validation rules as needed

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }
        }

        // Email validation helper
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }


}

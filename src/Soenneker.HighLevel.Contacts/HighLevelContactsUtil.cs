using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Soenneker.HighLevel.Contacts.Abstract;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.ValueTask;
using Soenneker.Extensions.Task;
using Soenneker.HighLevel.ClientUtil.Abstract;
using Soenneker.HighLevel.OpenApiClient;
using Soenneker.HighLevel.OpenApiClient.Models;

namespace Soenneker.HighLevel.Contacts;

/// <inheritdoc cref="IHighLevelContactsUtil" />
public sealed class HighLevelContactsUtil : IHighLevelContactsUtil
{
    private readonly IHighLevelClientUtil _highLevelClient;
    private readonly ILogger<HighLevelContactsUtil> _logger;

    private readonly bool _log;

    public HighLevelContactsUtil(IHighLevelClientUtil highLevelClient, ILogger<HighLevelContactsUtil> logger, IConfiguration config)
    {
        _highLevelClient = highLevelClient;
        _logger = logger;

        _log = config.GetValue<bool>("HighLevel:LogEnabled");
    }

    public async ValueTask<UpsertContactsSuccessfulResponseDto?> Upsert(string apiKey, UpsertContactDto contact, CancellationToken cancellationToken = default)
    {
        contact.Email = contact.Email?.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Upserting contact ({email}) to High Level location ({LocationId})...", contact.Email, contact.LocationId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(apiKey, cancellationToken).NoSync();

        return await client.Contacts.Upsert.PostAsync(contact, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<ContactsSearchSuccessfulResponseDto?> Search(string apiKey, ContactsSearchContactsAdvancedRequest searchBody,
        CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Searching for contacts in High Level...");

        HighLevelOpenApiClient client = await _highLevelClient.Get(apiKey, cancellationToken).NoSync();

        Stream? response = await client.Contacts.Search.PostAsync(searchBody, config => { }, cancellationToken).NoSync();

        if (response is null)
            return null;

        await using (response)
        {
            var factory = new JsonParseNodeFactory();
            IParseNode root = await factory.GetRootParseNodeAsync("application/json", response, cancellationToken).NoSync();
            return root.GetObjectValue(ContactsSearchSuccessfulResponseDto.CreateFromDiscriminatorValue);
        }
    }

    public async ValueTask<ContactsByIdSuccessfulResponseDto?> GetById(string apiKey, string contactId, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Getting contact from High Level with ID ({ContactId})...", contactId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(apiKey, cancellationToken).NoSync();

        return await client.Contacts[contactId].GetAsync(config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<ContactsSearchSchema?> GetByEmail(string apiKey, string email, string locationId, CancellationToken cancellationToken = default)
    {
        email = email.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Getting contact from High Level with email ({Email}) in location ({LocationId})...", email, locationId);

        var searchBody = new ContactsSearchContactsAdvancedRequest
        {
            AdditionalData =
            {
                ["locationId"] = locationId,
                ["query"] = email,
                ["limit"] = 1
            }
        };

        ContactsSearchSuccessfulResponseDto? response = await Search(apiKey, searchBody, cancellationToken).NoSync();
        return response?.Contacts?.FirstOrDefault();
    }

    public async ValueTask<UpdateContactsSuccessfulResponseDto?> Update(string apiKey, string contactId, UpdateContactDto updateDto,
        CancellationToken cancellationToken = default)
    {
        updateDto.Email = updateDto.Email?.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Updating contact ({ContactId}) in High Level...", contactId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(apiKey, cancellationToken).NoSync();

        return await client.Contacts[contactId].PutAsync(updateDto, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<DeleteContactsSuccessfulResponseDto?> Delete(string apiKey, string contactId, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogWarning("Deleting contact ({ContactId}) from High Level...", contactId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(apiKey, cancellationToken).NoSync();

        return await client.Contacts[contactId].DeleteAsync(config => { }, cancellationToken).NoSync();
    }
}

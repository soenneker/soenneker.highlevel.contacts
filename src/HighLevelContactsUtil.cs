using Microsoft.Extensions.Logging;
using Soenneker.HighLevel.Contacts.Abstract;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.ValueTask;
using Soenneker.Extensions.Task;
using Soenneker.HighLevel.ClientUtil.Abstract;
using Soenneker.HighLevel.OpenApiClient;
using Soenneker.HighLevel.OpenApiClient.Contacts.Contacts.Search;
using Soenneker.HighLevel.OpenApiClient.Models;
using System.Linq;

namespace Soenneker.HighLevel.Contacts;

/// <inheritdoc cref="IHighLevelContactsUtil"/>
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

    public async ValueTask<UpsertContactsSuccessfulResponseDto?> Upsert(UpsertContactDto contact, CancellationToken cancellationToken = default)
    {
        contact.Email = contact.Email?.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Upserting contact ({email}) to High Level location ({LocationId})...", contact.Email, contact.LocationId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(cancellationToken).NoSync();

        return await client.Contacts.Contacts.Upsert.PostAsync(contact, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<SearchPostResponse?> Search(SearchBodyV2DTO searchBody, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Searching for contacts in High Level...");

        HighLevelOpenApiClient client = await _highLevelClient.Get(cancellationToken).NoSync();

        return await client.Contacts.Contacts.Search.PostAsync(searchBody, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<ContactsByIdSuccessfulResponseDto?> GetById(string contactId, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Getting contact from High Level with ID ({ContactId})...", contactId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(cancellationToken).NoSync();

        return await client.Contacts.Contacts[contactId].GetAsync(config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<GetContectByIdSchema?> GetByEmail(string email, string locationId, CancellationToken cancellationToken = default)
    {
        email = email.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Getting contact from High Level with email ({Email}) in location ({LocationId})...", email, locationId);

        var searchBody = new SearchBodyV2DTO();
        searchBody.AdditionalData["locationId"] = locationId;
        searchBody.AdditionalData["query"] = email;
        searchBody.AdditionalData["limit"] = 1;

        SearchPostResponse? response = await Search(searchBody, cancellationToken).NoSync();

        if (response?.AdditionalData == null)
            return null;

        // Try to extract contacts from response
        if (response.AdditionalData.TryGetValue("contacts", out object? contactsObj) && contactsObj is System.Collections.IEnumerable contacts)
        {
            var firstContact = contacts.Cast<object>().FirstOrDefault();
            if (firstContact is GetContectByIdSchema contact)
                return contact;
        }

        return null;
    }

    public async ValueTask<UpdateContactsSuccessfulResponseDto?> Update(string contactId, UpdateContactDto updateDto, CancellationToken cancellationToken = default)
    {
        updateDto.Email = updateDto.Email?.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Updating contact ({ContactId}) in High Level...", contactId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(cancellationToken).NoSync();

        return await client.Contacts.Contacts[contactId].PutAsync(updateDto, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<DeleteContactsSuccessfulResponseDto?> Delete(string contactId, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogWarning("Deleting contact ({ContactId}) from High Level...", contactId);

        HighLevelOpenApiClient client = await _highLevelClient.Get(cancellationToken).NoSync();

        return await client.Contacts.Contacts[contactId].DeleteAsync(config => { }, cancellationToken).NoSync();
    }
}

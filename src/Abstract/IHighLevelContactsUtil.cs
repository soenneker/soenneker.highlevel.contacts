using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.HighLevel.OpenApiClient.Contacts.Contacts.Search;
using Soenneker.HighLevel.OpenApiClient.Models;

namespace Soenneker.HighLevel.Contacts.Abstract;

/// <summary>
/// A .NET typesafe implementation of High Level's contact API
/// </summary>
public interface IHighLevelContactsUtil
{
    /// <summary>
    /// Upserts a contact (creates or updates based on email/phone).
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contact">The contact data to upsert.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the upsert response, or null if the operation fails.</returns>
    ValueTask<UpsertContactsSuccessfulResponseDto?> Upsert(string apiKey, UpsertContactDto contact, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for contacts based on advanced filters.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="searchBody">The search criteria containing filters for the contact search.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous search operation. The task result contains the search response, or null if the operation fails.</returns>
    ValueTask<SearchPostResponse?> Search(string apiKey, SearchBodyV2DTO searchBody, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single contact by ID.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contactId">The unique identifier of the contact to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the contact if found, or null if not found.</returns>
    ValueTask<ContactsByIdSuccessfulResponseDto?> GetById(string apiKey, string contactId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single contact by email address.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="email">The email address of the contact to retrieve.</param>
    /// <param name="locationId">The location ID to search within.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the contact if found, or null if not found.</returns>
    ValueTask<GetContectByIdSchema?> GetByEmail(string apiKey, string email, string locationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing contact by ID.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contactId">The unique identifier of the contact to update.</param>
    /// <param name="updateDto">The contact data to update.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the update response, or null if the operation fails.</returns>
    ValueTask<UpdateContactsSuccessfulResponseDto?> Update(string apiKey, string contactId, UpdateContactDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a contact by ID.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contactId">The unique identifier of the contact to delete.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains the delete response, or null if the operation fails.</returns>
    ValueTask<DeleteContactsSuccessfulResponseDto?> Delete(string apiKey, string contactId, CancellationToken cancellationToken = default);
}

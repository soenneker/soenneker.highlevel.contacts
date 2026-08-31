using System.Threading;
using System.Threading.Tasks;
using Soenneker.HighLevel.OpenApiClient.Models;

namespace Soenneker.HighLevel.Contacts.Abstract;

/// <summary>
/// Provides typed HighLevel contact operations, including upsert, search, lookup, update, and delete.
/// </summary>
public interface IHighLevelContactsUtil
{
    /// <summary>
    /// Upserts a contact (creates or updates based on email/phone).
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contact">The contact data to upsert.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response body, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<UpsertContactsSuccessfulResponseDto?> Upsert(string apiKey, UpsertContactDto contact, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for contacts based on advanced filters.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="searchBody">The search criteria containing filters for the contact search.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The search response, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<ContactsSearchSuccessfulResponseDto?> Search(string apiKey, ContactsSearchContactsAdvancedRequest searchBody,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single contact by ID.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contactId">The unique identifier of the contact to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response body, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<ContactsByIdSuccessfulResponseDto?> GetById(string apiKey, string contactId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single contact by email address.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="email">The email address of the contact to retrieve.</param>
    /// <param name="locationId">The location ID to search within.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first matching contact, or <see langword="null"/> when none is returned.</returns>
    ValueTask<ContactsSearchSchema?> GetByEmail(string apiKey, string email, string locationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing contact by ID.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contactId">The unique identifier of the contact to update.</param>
    /// <param name="updateDto">The contact data to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response body, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<UpdateContactsSuccessfulResponseDto?> Update(string apiKey, string contactId, UpdateContactDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a contact by ID.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="contactId">The unique identifier of the contact to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response body, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<DeleteContactsSuccessfulResponseDto?> Delete(string apiKey, string contactId, CancellationToken cancellationToken = default);
}

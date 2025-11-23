using AuthLibrary.Models;

namespace AuthLibrary.Repository
{
    /// <summary>
    /// Interface defining security and credential management operations for customers.
    /// </summary>
    public interface ICustomerSecurityRepository
    {
        /// <summary>
        /// Inserts security data for a new customer into the system.
        /// </summary>
        /// <param name="userData">Object containing the user's registration information.</param>
        /// <param name="customerID">ID of the customer associated with the security data.</param>
        /// <returns>
        /// Returns <c>true</c> if the insertion was successful; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> InsertCustomerSecurityAsync(UserRegisterRequest userData, int customerID);

        /// <summary>
        /// Updates the password of an existing customer.
        /// </summary>
        /// <param name="customerId">ID of the customer whose password should be updated.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>
        /// Returns <c>true</c> if the update was successful; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> UpdatePasswordAsync(int customerId, string newPassword);

        /// <summary>
        /// Retrieves security data of a customer based on their email.
        /// </summary>
        /// <param name="emailAddress">Email of the customer to search for.</param>
        /// <returns>
        /// Returns a <see cref="UserSecurityData"/> object if the customer is found; otherwise, <c>null</c>.
        /// </returns>
        public Task<UserSecurityData?> GetCustomerSecurityByEmailAsync(string emailAddress);

        /// <summary>
        /// Deletes the security data of an existing customer.
        /// </summary>
        /// <param name="customerID">ID of the customer to delete.</param>
        /// <returns>
        /// Returns <c>true</c> if the deletion was successful; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> DeleteCustomerSecurityAsync(int customerID);

        /// <summary>
        /// Rolls back the security data of a customer in case of an error
        /// or incomplete operations.
        /// </summary>
        /// <param name="userData">Object containing the security data to restore.</param>
        public Task RollbackCustomerSecurity(UserSecurityData userData);

        public Task<bool> UpdateEmailSecurity(int customerID, string newEmail);


    }
}

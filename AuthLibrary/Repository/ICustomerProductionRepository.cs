using AuthLibrary.Models;

namespace AuthLibrary.Repository
{
    /// <summary>
    /// Interface defining operations for managing customer production records.
    /// </summary>
    public interface ICustomerProductionRepository
    {
        /// <summary>
        /// Asynchronously inserts a new customer production record using the provided user registration data.
        /// </summary>
        /// <param name="userData">The user registration data used to create the customer production record. Cannot be null.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains the unique identifier of the newly inserted
        /// customer production record.
        /// </returns>
        Task<int> InsertCustomerProductionAsync(UserRegisterRequest userData);

        /// <summary>
        /// Asynchronously deletes a customer production record by its unique identifier.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer production record to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result is <c>true</c> if the deletion was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteCustomerProductionAsync(int customerID);

        /// <summary>
        /// Asynchronously retrieves a customer production record by its unique identifier.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer production record to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains the <see cref="UserProductionData"/> 
        /// if the record is found; otherwise, <c>null</c>.
        /// </returns>
        Task<UserProductionData?> GetCustomerProductionByIdAsync(int customerID);

    }
}

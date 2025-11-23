using AuthLibrary.Models;

namespace AuthLibrary.Repository
{
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
        /// A task representing the asynchronous operation. The result is true if the deletion was successful; otherwise, false.
        /// </returns>
        Task<bool> DeleteCustomerProductionAsync(int customerID);

        /// <summary>
        /// Asynchronously retrieves a customer production record by its unique identifier.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer production record to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains the <see cref="UserProductionData"/> 
        /// if found; otherwise, null.
        /// </returns>
        Task<UserProductionData?> GetCustomerProductionByIdAsync(int customerID);
    }
}

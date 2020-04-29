using Abarnathy.DemographicsAPI.Models;

namespace Abarnathy.DemographicsAPI.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        // TODO: better method names

        /// <summary>
        /// Determines value equivalence between two <see cref="Patient"/> entities 
        /// by comparing the GivenName, FamilyName, SexId, and DateOfBirth properties.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="obj">The object against which to compare.</param>
        /// <returns>The boolean result of the value comparison.</returns>
        public static bool ValueEquals(this Patient entity, object obj)
        {
            if (obj is null)
            {
                return false;
            }

            // No point in comparing an object against itself
            if (ReferenceEquals(entity, obj))
            {
                return true;
            }

            // No point in comparing an object against an objet of a different type
            if (obj.GetType() != entity.GetType())
            {
                return false;
            }

            var @object = obj as Patient;

            return
                (NormaliseString(entity.GivenName) == NormaliseString(@object.GivenName)) &&
                (NormaliseString(entity.FamilyName) == NormaliseString(@object.FamilyName)) &&
                entity.DateOfBirth.Date.Equals(@object.DateOfBirth.Date);
        }

        /// <summary>
        /// Determine value equivalence between two <see cref="Address"/> entities 
        /// by comparing the StreetName, HouseNumber, Town, State, and ZipCode properties.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="obj">The object against which to compare.</param>
        /// <returns>The boolean result of the value comparison.</returns>
        public static bool ValueEquals(this Address entity, object obj)
        {
            if (obj is null)
            {
                return false;
            }

            // No point in comparing an object against itself
            if (ReferenceEquals(entity, obj))
            {
                return true;
            }

            // No point in comparing an object against an objet of a different type
            if (obj.GetType() != entity.GetType())
            {
                return false;
            }

            var @object = obj as Address;

            return
                (NormaliseString(entity.StreetName) == NormaliseString(@object.StreetName)) &&
                (NormaliseString(entity.HouseNumber) == NormaliseString(@object.HouseNumber)) &&
                (NormaliseString(entity.Town) == NormaliseString(@object.Town)) &&
                (NormaliseString(entity.State) == NormaliseString(@object.State)) &&
                (NormaliseString(entity.Zipcode) == NormaliseString(@object.Zipcode));
        }

        /// <summary>
        /// Determine value equivalence between two <see cref="PhoneNumber"/> entities 
        /// by comparing the Number property.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="obj">The object against which to compare.</param>
        /// <returns>The boolean result of the value comparison.</returns>
        public static bool ValueEquals(this PhoneNumber entity, object obj)
        {
            if (obj is null)
            {
                return false;
            }

            // No point in comparing an object against itself
            if (ReferenceEquals(entity, obj))
            {
                return true;
            }

            // No point in comparing an object against an objet of a different type
            if (obj.GetType() != entity.GetType())
            {
                return false;
            }

            var @object = obj as PhoneNumber;

            return NormaliseString(entity.Number) == NormaliseString(@object.Number);

        }


        /**
         * Helper methods
         * 
         */

        /// <summary>
        /// Given a non-null s
        /// </summary>
        private static string NormaliseString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str.Trim().ToLower();
        }
    }
}

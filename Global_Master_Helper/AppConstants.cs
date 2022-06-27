using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper
{
    public static class AppConstants
    {
        public const string XmlRootTag = "ROWSET";
        public const string XmlObjectRootTag = "ROW";
        public const string SaveMessage = "Data saved successfully.";
        public const string EditMessage = "Data edited successfully.";
        public const string DeleteMessage = "Data deleted successfully.";
        public const string DuplicateEntrMessage = "Same entry exists in database.";
        public const string ErrorMessage = "Error occured while saving data";
        public const string DatetimeFormat = "dd-MMM-yyyy";
        public const string DependancyMessage = "Data already used in Transaction.Can not be deleted.";
        public const string MandatoryMessage = "Please fill up all the mandatory fields";
        public const string Error = "Error occured";
    }
}

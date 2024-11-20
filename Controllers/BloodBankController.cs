using Microsoft.AspNetCore.Mvc;
using BloodBankManagement.Models;
using System.Text.RegularExpressions;


namespace BloodBankManagement.Controllers
{
    [ApiController]
    [Route("api/bloodbank")]
    public class BloodBankController : ControllerBase
    {
        private static readonly List<BloodBankEntry> _bloodBankEntries = new();
        private static int _id=0;
        private string[] validBloodTypes = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
        private List<string> validStatuses = new List<string> { "Available", "Requested", "Expired" };

        // POST: Create a new blood bank entry
        [HttpPost]
        public IActionResult Create([FromBody] BloodBankEntry entry)
        {
            entry.DonorName = entry.DonorName.Trim();
            entry.ContactInfo = entry.ContactInfo.Trim();
            entry.Status = entry.Status.Trim();
            entry.BloodType = entry.BloodType.Trim();
            // Validate DonorName
            if (string.IsNullOrWhiteSpace(entry.DonorName))
                return BadRequest("DonorName is required and cannot be empty.");

            // Check if donor name contains only valid characters (e.g., letters, spaces, hyphens)
            if (!Regex.IsMatch(entry.DonorName, @"^[a-zA-Z\s-]+$"))
                return BadRequest("Donor name can only contain letters, spaces, and hyphens.");

            // Ensure the name is between 3 and 100 characters
            if (entry.DonorName.Length < 3 || entry.DonorName.Length > 100)
                return BadRequest("DonorName must be between 3 and 100 characters.");

            // Ensure the name consists of 1 or 2 words (first name and last name)
            var namePattern = @"^[A-Za-z]+(?:\s[A-Za-z]+)?$";  // Matches 1 or 2 words with letters only
            if (!Regex.IsMatch(entry.DonorName, namePattern))
                return BadRequest("DonorName must contain one or two words (first and last name) with .");


            // Validate Age
            if (entry.Age < 18 || entry.Age > 65)
                return BadRequest("Age must be between 18 and 65.");

            // Validate BloodType
            
            if (!validBloodTypes.Contains(entry.BloodType))
                return BadRequest($"BloodType must be one of the following: {string.Join(", ", validBloodTypes)}.");

            // Validate ContactInfo
            if (string.IsNullOrWhiteSpace(entry.ContactInfo) || !Regex.IsMatch(entry.ContactInfo, @"^\d{10}$"))
                return BadRequest("ContactInfo is required and must be a valid 10-digit phone number.");


            // Validate Quantity
            if (entry.Quantity <= 0 || entry.Quantity>500)
                return BadRequest("Quantity must be a positive value.");

            // Validate CollectionDate
            if (entry.CollectionDate == default || entry.CollectionDate > DateTime.Now)
                return BadRequest("CollectionDate must be a valid date and cannot be in the future.");
             
            // Validate ExpirationDate
            if (entry.ExpirationDate == default || entry.ExpirationDate <= entry.CollectionDate)
                return BadRequest("ExpirationDate must be a valid date and later than the CollectionDate.");

            // Validate Status
            var validStatuses = new[] { "Available", "Requested", "Expired" };
            if (!validStatuses.Contains(entry.Status))
                return BadRequest($"Status must be one of the following: {string.Join(", ", validStatuses)}.");

            // Generate a new integer Id
            entry.Id = _id;
            _id=_id+1;
            // Console.WriteLine(_id);

            // Add entry to the list
            _bloodBankEntries.Add(entry);

            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
        }


        // GET: Retrieve all entries
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_bloodBankEntries);
        }

        // GET: Retrieve a specific entry by Id
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entry = _bloodBankEntries.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound("Entry not found.");
            }
            return Ok(entry);
        }


        // PUT: Update an existing entry
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] BloodBankEntry updatedEntry)
        {
            // DonorName Validation
            if (string.IsNullOrWhiteSpace(updatedEntry.DonorName))
                return BadRequest("DonorName is required and cannot be empty.");

            if (updatedEntry.DonorName.Length < 3 || updatedEntry.DonorName.Length > 100)
                return BadRequest("DonorName must be between 3 and 100 characters.");


            var namePattern = @"^[A-Za-z]+(?:\s[A-Za-z]+)?$"; 
            if (!Regex.IsMatch(updatedEntry.DonorName, namePattern))
                return BadRequest("DonorName must contain one or two words (first and last name) without extra spaces.");


            // Age Validation
            if (updatedEntry.Age < 18 || updatedEntry.Age > 65)
                return BadRequest("Age must be between 18 and 65.");

            // BloodType Validation
            if (!validBloodTypes.Contains(updatedEntry.BloodType))
                return BadRequest($"BloodType must be one of the following: {string.Join(", ", validBloodTypes)}.");

            // ContactInfo Validation
            if (string.IsNullOrWhiteSpace(updatedEntry.ContactInfo) || !Regex.IsMatch(updatedEntry.ContactInfo, @"^\d{10}$"))
                return BadRequest("ContactInfo is required and must be a valid 10-digit phone number.");

            // Quantity Validation
            if (updatedEntry.Quantity <= 0 || updatedEntry.Quantity > 500)
                return BadRequest("Quantity must be a positive value and not exceed 500 ml.");

            // CollectionDate Validation
            if (updatedEntry.CollectionDate == default || updatedEntry.CollectionDate > DateTime.Now)
                return BadRequest("CollectionDate must be a valid date and cannot be in the future.");

            // ExpirationDate Validation
            if (updatedEntry.ExpirationDate == default || updatedEntry.ExpirationDate <= updatedEntry.CollectionDate)
                return BadRequest("ExpirationDate must be a valid date and later than the CollectionDate.");

            // Status Validation
            if (!validStatuses.Contains(updatedEntry.Status))
                return BadRequest($"Status must be one of the following: {string.Join(", ", validStatuses)}.");

            var entry = _bloodBankEntries.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound("Entry not found.");
            }

            entry.DonorName = updatedEntry.DonorName;
            entry.Age = updatedEntry.Age;
            entry.BloodType = updatedEntry.BloodType;
            entry.ContactInfo = updatedEntry.ContactInfo;
            entry.Quantity = updatedEntry.Quantity;
            entry.CollectionDate = updatedEntry.CollectionDate;
            entry.ExpirationDate = updatedEntry.ExpirationDate;
            entry.Status = updatedEntry.Status;

            return Ok(entry);
        }


        // DELETE: Remove an entry by Id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entry = _bloodBankEntries.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound("Entry not found.");
            }

            _bloodBankEntries.Remove(entry);
            return NoContent();
        }


        // GET: Pagination
        [HttpGet]
        [Route("pagination")]
        public IActionResult GetPaginated(int page = 1, int size = 10)
        {
            // page number Validation
            if (page < 1)
            {
                return BadRequest("Page number must be greater than or equal to 1.");
            }
            var paginatedEntries = _bloodBankEntries
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return Ok(paginatedEntries);
        }

        // GET: Search by blood type
        [HttpGet("search/bloodtype")]
        public IActionResult SearchByBloodType([FromQuery] string bloodType)
        {
            if (string.IsNullOrEmpty(bloodType))
                return BadRequest("Blood type is required.");
            bloodType = bloodType.Trim();
            if (!validBloodTypes.Contains(bloodType))
                return BadRequest($"BloodType must be one of the following: {string.Join(", ", validBloodTypes)}.");

            var result = _bloodBankEntries
                .Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
                return NotFound("No entries found for the specified blood type.");

            return Ok(result);
        }

        // GET: Search by status
        [HttpGet("search/status")]
        public IActionResult SearchByStatus([FromQuery] string status)
        {


            // status Validation
            if (string.IsNullOrEmpty(status))
                return BadRequest("Status is required.");


            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                return BadRequest($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}.");

            var result = _bloodBankEntries
                .Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
                return NotFound("No entries found for the specified status.");

            return Ok(result);
        }


        // GET: Search by donor name
        [HttpGet("search/donorname")]
        public IActionResult SearchByDonorName([FromQuery] string donorName)
        {
            // donor name Validation
            if (string.IsNullOrEmpty(donorName))
                return BadRequest("Donor name is required.");

            if (donorName.Length < 2 || donorName.Length > 100)
                return BadRequest("Donor name must be between 2 and 100 characters.");

            if (!Regex.IsMatch(donorName, @"^[a-zA-Z\s-]+$"))
                return BadRequest("Donor name can only contain letters, spaces, and hyphens.");

            var nameParts = donorName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length < 2 || nameParts.Length > 2)
                return BadRequest("Donor name must contain exactly two words (first name and last name).");

            var result = _bloodBankEntries
                .Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
                return NotFound("No donors found with the specified name.");

            return Ok(result);
        }



        // Filtering API
        [HttpGet("filter")]
        public IActionResult Filter(
            [FromQuery] string bloodType = null,
            [FromQuery] string status = null,
            [FromQuery] string donorName = null)
        {
            var result = _bloodBankEntries.AsQueryable();

            
            if (!string.IsNullOrEmpty(bloodType) &&   !validBloodTypes.Contains(bloodType))
                return BadRequest($"BloodType must be one of the following: {string.Join(", ", validBloodTypes)}.");

            if (!string.IsNullOrEmpty(bloodType))
                bloodType = bloodType.Trim();
                result = result.Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));


            // status Validation
            if (!string.IsNullOrEmpty(status) && !validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                return BadRequest($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}.");
            if (!string.IsNullOrEmpty(status))
                result = result.Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase));


            if (!string.IsNullOrEmpty(donorName) && donorName.Length < 2 || donorName.Length > 100)
                return BadRequest("Donor name must be between 2 and 100 characters.");

            if (!string.IsNullOrEmpty(donorName) && !Regex.IsMatch(donorName, @"^[a-zA-Z\s-]+$"))
                return BadRequest("Donor name can only contain letters, spaces, and hyphens.");

            var nameParts = donorName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(donorName) && nameParts.Length < 2 || nameParts.Length > 2)
                return BadRequest("Donor name must contain exactly two words (first name and last name).");
            if (!string.IsNullOrEmpty(donorName))
                result = result.Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase));

            if (!result.Any())
                return NotFound("No matching entries found.");

            return Ok(result.ToList());
        }

        // Sorting API
        [HttpGet("sort")]
        public IActionResult Sort(
            [FromQuery] string sortBy,
            [FromQuery] bool ascending = true)
        {
            if (string.IsNullOrEmpty(sortBy))
                return BadRequest("The sortBy parameter is required.");

            var validSortByOptions = new HashSet<string> { "bloodtype", "collectiondate" };

            sortBy = sortBy.ToLower();

            if (!validSortByOptions.Contains(sortBy))
                return BadRequest("Invalid sortBy value. Supported values: 'bloodtype', 'collectiondate'.");

            var result = _bloodBankEntries.AsQueryable();

            result = sortBy switch
            {
                "bloodtype" => ascending ? result.OrderBy(e => e.BloodType) : result.OrderByDescending(e => e.BloodType),
                "collectiondate" => ascending ? result.OrderBy(e => e.CollectionDate) : result.OrderByDescending(e => e.CollectionDate),
                _ => result
            };

            if (!result.Any())
                return NotFound("No entries available for sorting.");

            return Ok(result.ToList());
        }



    }


}

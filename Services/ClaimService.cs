using TheContractMonthlyClaimSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace TheContractMonthlyClaimSystem.Services
{
    public class ClaimService
    {
        private readonly List<Claims> _claims = new List<Claims>();
        private int _nextId = 1;

        public void AddClaim(Claims claim)
        {
            claim.Id = _nextId++;
            claim.Submit();
            _claims.Add(claim);
        }

        public List<Claims> GetAllClaims()
        {
            return _claims.OrderByDescending(c => c.SubmittedAt).ToList();
        }

        public List<Claims> GetPendingClaims()
        {
            return _claims.Where(c => c.Status == "Submitted" || c.Status == "Pending").ToList();
        }

        public Claims GetClaimById(int id)
        {
            return _claims.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateClaimStatus(int id, string status)
        {
            var claim = GetClaimById(id);
            if (claim != null)
            {
                claim.UpdateStatus(status);
            }
        }

        public void AddDocumentToClaim(int claimId, Document document)
        {
            var claim = GetClaimById(claimId);
            if (claim != null)
            {
                claim.Documents.Add(document);
                claim.LastUpdated = DateTime.Now;
            }
        }

        // Get document by claim ID and file name
        public Document GetDocument(int claimId, string fileName)
        {
            var claim = GetClaimById(claimId);
            return claim?.Documents.FirstOrDefault(d => d.FileName == fileName);
        }

        // Get all documents for a claim
        public List<Document> GetClaimDocuments(int claimId)
        {
            var claim = GetClaimById(claimId);
            return claim?.Documents ?? new List<Document>();
        }
    }
}
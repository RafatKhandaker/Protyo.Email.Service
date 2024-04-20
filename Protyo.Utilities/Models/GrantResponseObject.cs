using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Models
{
    public class Grants
    {
        public SearchParams searchParams { get; set; }
        public int? hitCount { get; set; }
        public int? startRecord { get; set; }
        public List<OppHit> oppHits { get; set; }
        public List<OppStatusOption> oppStatusOptions { get; set; }
        public List<DateRangeOption> dateRangeOptions { get; set; }
        public string suggestion { get; set; }
        public List<Eligibility> eligibilities { get; set; }
        public List<FundingCategory> fundingCategories { get; set; }
        public List<FundingInstrument> fundingInstruments { get; set; }
        public List<Agency> agencies { get; set; }
        public string accessKey { get; set; }
        public List<object> errorMsgs { get; set; }

    }

    public class Agency
    {
        public List<SubAgencyOption> subAgencyOptions { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }

    public class DateRangeOption
    {
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }

    public class Eligibility
    {
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }

    public class FundingCategory
    {
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }

    public class FundingInstrument
    {
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }

    public class OppHit
    {
        public int? id { get; set; }
        public string number { get; set; }
        public string title { get; set; }
        public string agencyCode { get; set; }
        public string agency { get; set; }
        public string openDate { get; set; }
        public string closeDate { get; set; }
        public string oppStatus { get; set; }
        public string docType { get; set; }
        public List<string> cfdaList { get; set; }
    }

    public class OppStatusOption
    {
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }


    public class SearchParams
    {
        public string resultType { get; set; }
        public bool? searchOnly { get; set; }
        public string sortBy { get; set; }
        public string dateRange { get; set; }
        public string oppStatuses { get; set; }
        public int? startRecordNum { get; set; }
        public int? rows { get; set; }
        public bool? keywordEncoded { get; set; }
    }

    public class SubAgencyOption
    {
        public string label { get; set; }
        public string value { get; set; }
        public int? count { get; set; }
    }

    public class GrantDetails
    {
        public int? id { get; set; }
        public int? revision { get; set; }
        public string opportunityNumber { get; set; }
        public string opportunityTitle { get; set; }
        public string owningAgencyCode { get; set; }
        public string listed { get; set; }
        public string publisherUid { get; set; }
        public string flag2006 { get; set; }
        public OpportunityCategory opportunityCategory { get; set; }
        public Synopsis synopsis { get; set; }
        public AgencyDetails agencyDetails { get; set; }
        public TopAgencyDetails topAgencyDetails { get; set; }
        public List<SynopsisAttachmentFolder> synopsisAttachmentFolders { get; set; }
        public List<object> synopsisDocumentURLs { get; set; }
        public List<object> synAttChangeComments { get; set; }
        public List<Cfda> cfdas { get; set; }
        public List<object> opportunityHistoryDetails { get; set; }
        public List<OpportunityPkg> opportunityPkgs { get; set; }
        public List<object> closedOpportunityPkgs { get; set; }
        public string originalDueDate { get; set; }
        public string originalDueDateDesc { get; set; }
        public List<object> synopsisModifiedFields { get; set; }
        public List<object> forecastModifiedFields { get; set; }
        public List<object> errorMessages { get; set; }
        public bool? synPostDateInPast { get; set; }
        public string docType { get; set; }
        public int? forecastHistCount { get; set; }
        public int? synopsisHistCount { get; set; }
        public bool? assistCompatible { get; set; }
        public string assistURL { get; set; }
        public List<object> relatedOpps { get; set; }
        public string draftMode { get; set; }
    }

    public class AgencyDetails
    {
        public string code { get; set; }
        public string seed { get; set; }
        public string agencyName { get; set; }
        public string agencyCode { get; set; }
        public string topAgencyCode { get; set; }
    }

    public class ApplicantType
    {
        public string id { get; set; }
        public string description { get; set; }
    }

    public class Cfda
    {
        public int? id { get; set; }
        public int? opportunityId { get; set; }
        public string cfdaNumber { get; set; }
        public string programTitle { get; set; }
    }

    public class FundingActivityCategory
    {
        public string id { get; set; }
        public string description { get; set; }
    }

    public class OpportunityCategory
    {
        public string category { get; set; }
        public string description { get; set; }
    }

    public class OpportunityPkg
    {
        public int? id { get; set; }
        public int? topportunityId { get; set; }
        public int? familyId { get; set; }
        public string dialect { get; set; }
        public string opportunityNumber { get; set; }
        public string opportunityTitle { get; set; }
        public string cfdaNumber { get; set; }
        public string openingDate { get; set; }
        public string closingDate { get; set; }
        public string owningAgencyCode { get; set; }
        public AgencyDetails agencyDetails { get; set; }
        public TopAgencyDetails topAgencyDetails { get; set; }
        public string programTitle { get; set; }
        public string contactInfo { get; set; }
        public int? gracePeriod { get; set; }
        public string competitionId { get; set; }
        public string competitionTitle { get; set; }
        public string electronicRequired { get; set; }
        public int? expectedApplicationCount { get; set; }
        public int? expectedApplicationSize { get; set; }
        public int? openToApplicantType { get; set; }
        public string listed { get; set; }
        public string isMultiProject { get; set; }
        public string extension { get; set; }
        public string mimetype { get; set; }
        public string lastUpdate { get; set; }
        public string workspaceCompatibleFlag { get; set; }
        public string packageId { get; set; }
        public string openingDateStr { get; set; }
        public string closingDateStr { get; set; }
    }

    public class Synopsis
    {
        public int? opportunityId { get; set; }
        public int? version { get; set; }
        public string agencyCode { get; set; }
        public string agencyName { get; set; }
        public string agencyPhone { get; set; }
        public string agencyAddressDesc { get; set; }
        public AgencyDetails agencyDetails { get; set; }
        public TopAgencyDetails topAgencyDetails { get; set; }
        public string agencyContactPhone { get; set; }
        public string agencyContactName { get; set; }
        public string agencyContactDesc { get; set; }
        public string agencyContactEmail { get; set; }
        public string agencyContactEmailDesc { get; set; }
        public string synopsisDesc { get; set; }
        public string responseDate { get; set; }
        public string responseDateDesc { get; set; }
        public string fundingDescLinkUrl { get; set; }
        public string fundingDescLinkDesc { get; set; }
        public string postingDate { get; set; }
        public bool? costSharing { get; set; }
        public string numberOfAwards { get; set; }
        public string estimatedFunding { get; set; }
        public string estimatedFundingFormatted { get; set; }
        public string awardCeiling { get; set; }
        public string awardCeilingFormatted { get; set; }
        public string awardFloor { get; set; }
        public string awardFloorFormatted { get; set; }
        public string applicantEligibilityDesc { get; set; }
        public string createTimeStamp { get; set; }
        public string createdDate { get; set; }
        public string lastUpdatedDate { get; set; }
        public List<ApplicantType> applicantTypes { get; set; }
        public List<FundingInstrument> fundingInstruments { get; set; }
        public List<FundingActivityCategory> fundingActivityCategories { get; set; }
        public string responseDateStr { get; set; }
        public string postingDateStr { get; set; }
        public string createTimeStampStr { get; set; }
    }

    public class SynopsisAttachment
    {
        public int? id { get; set; }
        public int? opportunityId { get; set; }
        public string mimeType { get; set; }
        public string fileName { get; set; }
        public string fileDescription { get; set; }
        public int? fileLobSize { get; set; }
        public string createdDate { get; set; }
        public int? synopsisAttFolderId { get; set; }
    }

    public class SynopsisAttachmentFolder
    {
        public int? id { get; set; }
        public int? opportunityId { get; set; }
        public string folderType { get; set; }
        public string folderName { get; set; }
        public int? zipLobSize { get; set; }
        public string createdDate { get; set; }
        public string lastUpdatedDate { get; set; }
        public List<SynopsisAttachment> synopsisAttachments { get; set; }
    }

    public class TopAgencyDetails
    {
        public string code { get; set; }
        public string seed { get; set; }
        public string agencyName { get; set; }
        public string agencyCode { get; set; }
        public string topAgencyCode { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
	public class MirysListModels
	{
		public DateTime DateOfEnrollment { get; set; }
		public int FamilyRecordNumber { get; set; }
		public string FamilyName { get; set; }
		public string HeadOfHousHoldFirstName { get; set; }
		public string SpouseFirstName { get; set; }
		public string CountryOfOrigin { get; set; }
		public DateTime USAArrivalDate { get; set; }
		public Address USAddress { get; set; }
		public string PrimaryContactPhoneNumber { get; set; }
		public string ResettlementCity { get; set; }
		public string FamilyPictureLink { get; set; }
		
		public string PrimaryEmail { get; set; }
		public string PrimaryLanguage { get; set; }
		public bool SpeaksEnglish { get; set; }
		public int NumberOfFamilyMembers { get; set; }
		public int NumberOfChildren { get; set; }
		public int NumberOfBabies { get; set; }
		public DateTime ExpectingDate { get; set; }
		public string ReferredBy { get; set; }
		public Status Status { get; set; }
		public string FormerOccupation { get; set; }
		public string SpouseFormerOccupation { get; set; }
		public int AdvocateId { get; set; }
		public int ListMakerId { get; set; }
		public int ListId { get; set; }
		public List<FamilyMember> FamilyMembers { get; set; }
		public List<Item> Items { get; set; }
		public FamilyCategory Category { get; set; }
	}

	public class Advocate
	{
		public int Id { get; set; }
		public string AdovcateName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
	}

	public class ListMaker
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public List<int> Lists { get; set; }
		public string Supervisor { get; set; }
	}

	public class List
	{
		public int Id { get; set; }
		public int ListMakerId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime GoliveDate { get; set; }
		public DateTime LastUpdatedTime { get; set; }
		public string AmazonLink { get; set; }
	}

	public class Address
	{
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
	}

	public class FamilyMember
	{
		public string Name { get; set; }
		public Sex Sex { get; set; }
		public DateTime BirthDay { get; set; }
		public int Age { get; set; }
		public string TopSize { get; set; }
		public string BottomSize { get; set; }
		public double ShoeSize { get; set; }
		public string ShoeSizeCountry { get; set; }
	}

	public class Item
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public string Size { get; set; }
		public string Price { get; set; }
		public string SubTotal { get; set; }
		public int NumberOfItemsPurchased { get; set; }
		public int NumberOfItemsRemaining { get; set; }
		public string TotalAmountOfPurcahsedItems { get; set; }
		public ItemCategory Cateogry { get; set; }
	}

	public enum ItemCategory
	{
		Thrive,
		Survive,
		Hive
	}

	public enum Status
	{
		Refugee,
		Asylum
	}

	public enum FamilyCategory
	{
		WorkedWithUSMilitaryOrWorldAidOrgs,
		FamilyWithSpecialNeedsOrDisability,
		FamilyWithNewbornOrExpecting,
		FamilyWithYoungChildren,
		FamilyWithSingleParentHousehold,
		FamilyWithElementarySchoolKids,
		FamilyWithMiddleSchoolers,
		FamilyWithTeenagers,
		FamilyWithSeniorCitizens,
		FamilyWithTwinsOrMultiples,
		FamilyWithUrgentNeeds
	}

	public enum Sex
	{
		Female,
		Male
	}
}

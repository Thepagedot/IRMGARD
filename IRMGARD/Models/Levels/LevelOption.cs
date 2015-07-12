using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class LevelOption
	{
		private LevelElement element;
		public LevelElement Element
		{
			get { return element; }
			set { Element = element; }
		}

		private string letter;
		public string Letter
		{
			get { return letter; }
			set { Letter = letter; }
		}

		private string name;
		public string Name
		{
			get { return name; }
			set { Name = name; }
		}

		private bool isCorret;
		public bool IsCorret
		{
			get { return isCorret; }
			set { IsCorret = isCorret; }
		}

		private bool isShort;
		public bool IsShort
		{
			get { return isShort; }
			set { IsShort = isShort; }
		}

		private bool isLong;
		public bool IsLong
		{
			get { return isLong; }
			set { IsLong = isLong; }
		}

		private int correctPos;
		public int CorrectPos
		{
			get { return correctPos; }
			set { CorrectPos = correctPos; }
		}

		private int id;
		public int Id
		{
			get { return id; }
			set { Id = id; }
		}

		// needed for FourPictures
		public LevelOption(LevelElement element, bool isCorrect)
		{
			Element = element;
			IsCorret = isCorret;
		}

		// needed for FindMissingLetter
		public LevelOption(string letter, bool isShort, bool isLong, int correctPos)
		{
			Letter = letter;
			IsShort = isShort;
			IsLong = isLong;
			CorrectPos = correctPos;
		}

		// needed for HearMe, AbcRank
		public LevelOption(string name, LevelElement element)
		{
			Name = name;
			Element = element;
		}

		// needed for Memory
		public LevelOption(int id, string name, LevelElement element)
		{
			Id = id;
			Name = name;
			Element = element;
		}
	}
}


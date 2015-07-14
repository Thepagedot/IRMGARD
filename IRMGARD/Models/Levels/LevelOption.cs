using System;
using System.Collections.ObjectModel;

namespace IRMGARD.Models
{
	public class LevelOption
	{
		public LevelElement Element;
		public void setLevelElement(LevelElement element){
			this.Element = element;
		}
		public LevelElement getLevelElement(){
			return this.Element;
		}

		public String Letter;
		public void setLetter(String letter){
			this.Letter = letter;
		}
		public String getLetter(){
			return Letter;
		}

		public String Name;
		public void setName(String name){
			this.Name = name;
		}
		public String getName(){
			return Name;
		}

		public bool IsCorrect;
		public void setIsCorrect(bool isCorrect){
			this.IsCorrect = isCorrect;
		}
		public bool getIsCorrect(){
			return IsCorrect;
		}

		public bool IsShort;
		public void setIsShort(bool isShort){
			this.IsShort = isShort;
		}
		public bool getIsShort(){
			return IsShort;
		}

		public bool IsLong;
		public void setIsLong(bool isLong){
			this.IsLong = isLong;
		}
		public bool getIsLong(){
			return IsLong;
		}

		public int CorrectPos;
		public void setCorrectPos(int correctPos){
			this.CorrectPos = correctPos;
		}
		public int getCorrectPos(){
			return CorrectPos;
		}

		public int Id;
		public void setId(int id){
			this.Id = id;
		}
		public int getId(){
			return Id;
		}

		public LevelOption(){}

		// needed for FourPictures
		public LevelOption(LevelElement element, bool isCorrect)
		{
			this.setLevelElement(element);
			this.setIsCorrect(isCorrect);
		}

		// needed for FindMissingLetter
		public LevelOption(string letter, bool isShort, bool isLong, int correctPos)
		{
			this.setLetter(letter);
			this.setIsShort(isShort);
			this.setIsLong(isLong);
			this.setCorrectPos(correctPos);
		}

		// needed for HearMe, AbcRank
		public LevelOption(string name, LevelElement element)
		{
			this.setName(name);
			this.setLevelElement(element);
		}

		// needed for Memory
		public LevelOption(int id, string name, LevelElement element)
		{
			this.setId(id);
			this.setName(name);
			this.setLevelElement(element);
		}
	}
}


﻿using System;
using System.Collections.Generic;
using System.Text;
using WordFilterNS;

namespace ImageDescribeBot
{
    class Censorboard
    {
        private readonly WordFilter _wordFilter;

        private List<string> lstExtraAIFilter;
        private List<string> lstWikiPhraseFilter;
        private List<string> lstWikiCategFilter;
        private Dictionary<string, string> dictGenderNeturalize;

        public Censorboard()
        {
            _wordFilter = new WordFilter();

            // the original author didn't want the bot to show this kind of imagery!
            _wordFilter.AddWords(new string[] { "nazi", "hitler", "reich" });

            /* We can't trust Microsoft's algorithm to not be racist, so we should probably
             * make the bot avoid posting images with the following words in them.
             * We're not using wordfilter here because it would over-filter in some cases.
             * also filter "gun" because this is not the kind of content we want the bot to post
             * This is matched only against the caption generated by CVAPI.
            */
            lstExtraAIFilter = new List<string> { "ape", "apes", "monkey", "monkeys", "gun" };

            // Blacklisted phrases (instead of words) to blacklist certain phrases
            // in the wikimedia description
            lstWikiPhraseFilter = new List<string> { "comic strip", "logo", "biblical illustration", "church", "historical document" };

            // Blacklist some categories, just in case. These are matched on a substring
            // basis, against the page's categories and the titles of the wikipages using
            // the picture.
            lstWikiCategFilter = new List<string> { "september 11", "hitler", "nazi", "antisemit", "libel",
                      "apartheid", "racism", "lynching", "cartoons",
                      "holocaust", "auschwitz", "stereotypes", "flags", "porn",
                      "homophobia", "transphobia", "logos",
                      "scans from google books", "little nemo",
                      "stolperstein", "songbird specimens", "terror",
                      "bible illustrations", "jesuit symbols",
                      "christian symbols", "symbols of religion",
                      "symbols of islam", "jewish symbols", "pistols",
                      "corpse", "victim", "ultrasound" };

            dictGenderNeturalize = new Dictionary<string, string>()
            {
                { "woman", "person" },
                { "man", "person" },
                { "women", "people" },
                { "man's", "person's" },
                { "woman's", "person's" },
                { "mans", "persons" },
                { "womans", "persons" },
                { "men", "people" },
                { "guy", "person" },
                { "boy", "person" },
                { "girl", "person" },
                { "boys", "people" },
                { "girls", "people" },
                { "lady", "person" },
                { "ladies", "people" },
                { "gentleman", "person" },
                { "gentlemen", "people" },
                { "female", "" },
                { "male", "" },
                { "she", "they" },
                // It's probably more likely to say "woman and her phone" than
                // "someone gives a phone to her", so their is probably better
                // here. Would need more complex parsing to know for sure.
                { "her", "their" },
                { "hers", "theirs" },
                { "herself", "themself" },
                { "he", "they" },
                { "him", "them" },
                // It's more likely to give "man and his phone" than "this
                // phone is his", so "their" is better here than "theirs"
                { "his", "their" },
                { "himself", "themself" }
            };
        }

        public string GenderNeutralizeString(string phrase)
        {
            return string.Empty;
        }

        public bool IsBlacklisted(string phrase)
        {
            return _wordFilter.Blacklisted(phrase);
        }

        public bool ShouldFilterForPhrase(string sentence)
        {
            sentence = sentence.ToLower().Trim();
            return lstWikiPhraseFilter.Exists(word => sentence.Contains(word));
        }

        public bool ShouldFilterForCategory(string sentence)
        {
            sentence = sentence.ToLower().Trim();
            return lstWikiCategFilter.Exists(word => sentence.Contains(word));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSentence"></param>
        /// <returns>true, when one of the sentence contains a blacklisted category. Else, false.</returns>
        public bool ShouldFilterForCategory(List<string> lstSentence)
        {
            foreach(string categ in lstWikiCategFilter)
            {
                if(lstSentence.Exists(sentence => sentence.ToLower().Contains(categ)))
                {
                    return true;
                }
            }

            return false;            
        }
    }
}

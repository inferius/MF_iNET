using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INetCore.Core.Language.CSS.Styles;

namespace INetCore.Core.Language.CSS
{
    public class ClassesTree
    {
        public void ParseQueryToken(string token)
        {
            
        }
    }

    public class ClassNode
    {
        private readonly List<BaseStyle> _styles  = new List<BaseStyle>();

        public string Name { get; set; } = "";
        public BaseStyle[] Styles => _styles.ToArray();
        /// <summary>
        /// Urcuje zda je primy potomek rodice, tedy jestli vztah mezi jeho a rodicovskym stylem je .Parent > .Child
        /// </summary>
        public bool IsDirectParent { get; set; } = false;


        public List<BaseStyle> GetStyesList() => _styles;
    }

    public class ClassAttributeInfo
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = null;
        public ClassAttributeValueInfo ValueRelationship { get; set; }
    }

    public enum ClassAttributeValueInfo
    {
        Equal, // =
        StartWithWord, // |=
        StartWith, // ^=
        ContainsWord, // ~=
        Contains, // *=
        EndWith, // $=
        RegExp // $$= // neni ve specifikaci
    }
}

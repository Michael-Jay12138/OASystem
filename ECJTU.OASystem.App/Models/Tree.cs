using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECJTU.OASystem.App.Models
{
    public class Tree
    {
        public string id { get; set; }
        public string text { get; set; }
        public State state { get; set; }
        public List<Tree> children { get; set; }
    }
    public class State
    {
        public bool selected=false;
        public bool disabled = false;
        public bool opened = false;
    }
}
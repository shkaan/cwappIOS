using System;
using System.Collections;
using System.Collections.Generic;

namespace cwappIOS
{
    public class MainTableModel
    {
        public bool success { get; set; }
        public List<ModelFields> apiData { get; set; }
    }

    public class ModelFields
    {
        public int entryid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string created_by { get; set; }
        public string created_at { get; set; }
        public object updated_by { get; set; }
        public string updated_at { get; set; }
        public string sessionid { get; set; }
        public int? is_approved { get; set; }
                
    }

}

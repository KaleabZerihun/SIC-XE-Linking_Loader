/********************************************************************
*** NAME : Kaleab Zerihun
*** CLASS : CSc 354
*** ASSIGNMENT : Assignment 
*** DUE DATE : 12-13-24
*** INSTRUCTOR : Hamer
*********************************************************************
*** DESCRIPTION : Write
the linker/loader as a separate program.****
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zerihun5
{
    public class ESTAB
    {
        public string CSECT {  get; set; }
        public string SYMBOL { get; set; }
        public string ADDR { get; set; }
        public string CSADDR { get; set; }
        public string LDADDR { get; set; }
        public string LENGTH { get; set; }
        /********************************************************************
        *** METHOD: ESTAB
        *********************************************************************
        *** DESCRIPTION : Constructor for the ESTAB class
        *** INPUT ARGS : string CSECTs, string SYMBOLs, string ADDRs, string CSADDRs, string LDADDRs, string LENGTHs
        **** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : 
        *** RETURN : NA
        ********************************************************************/
        public ESTAB(string CSECTs, string SYMBOLs, string ADDRs, string CSADDRs, string LDADDRs, string LENGTHs)
        {
            CSECT = CSECTs;
            SYMBOL = SYMBOLs;
            ADDR = ADDRs;
            CSADDR = CSADDRs;
            LDADDR = LDADDRs;
            LENGTH = LENGTHs;

        }

    }
}

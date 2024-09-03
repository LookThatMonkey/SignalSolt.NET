using ClassLibrary2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TrusteeShipAOP.Core.Attribute;

namespace TrusteeShipAOP
{
    [ClassAspect]
    //public class Class1<T,U>
    public class Class1
    {

        public List<IShape> Shapes { get; private set; } = new List<IShape>();
        //public Class1(Panel c)
        //{
        //}
        //public Class1(Control c)
        //    : this()
        //{
        //}

        //public Class1()
        //{
        //    TrusteeShipAOP.Core.Environment.AddEntrty(this);
        //}
        public Class1(Control c)
        {
        }

        public int a1()
        {
            return default(int);
        }

        public Class1 a2()
        {
            return default(Class1);
        }

        public Point a3()
        {
            return default(Point);
        }





        static int s1 = 0;

        [DemoPropertyAspect]
        public int aabbc { get; set; } = 0;

        [DemoPropertyAspect]
        public int aabbd { get; set; } = 0;
        protected int ia = 0;
        int ic = 11;
        int ib = 22;
        [DemoPropertyAspect]
        [DemoMethodAspect]
        public void Add(int i1, int i2)
        {
            try
            {
                List<int> li = new List<int>();
                int iiiii = li[0];
                try
                {
                    AddA1();
                }
                catch (Exception ex)
                {
                }
                s1 = 2;
                ib = s1;
                aa(0, 0);
                aa2(0, 0);
            }
            catch
            { 
            }
            //aabbc = 1;
            //Add2(0, 0);
        }

        [DemoMethodAspect]
        public void AddA1()
        {
            List<int> li = new List<int>();
            int iiiii = li[0];
        }
        [DemoMethodAspect]
        public void AddA12()
        {
        }
        private int aa(int i1, float f2)
        {
            return 123;
        }
        private int aadd(int i1, float f2)
        {
            throw new Exception();
        }

        public static int aa2(int i1, float f2)
        {
            return 123;
        }
    }

    class DemoMethodAspectAttribute : Core.Attribute.MPAspectAttribute
    {
        public override void OnEntry(object sender, Core.Attribute.AspectEventArgs args)
        {
            args.MethodName = "OnEntry " + args.MethodName + "  " + args.Params;
            Form1.OnEntry?.Invoke(sender, args);
        }

        public override void OnExit(object sender, Core.Attribute.AspectEventArgs args)
        {
            args.MethodName = "OnExit " + args.MethodName + "  " + args.Params;
            Form1.OnExit?.Invoke(sender, args);
        }

        public override bool Validating(object sender, Core.Attribute.AspectEventArgs args)
        {
            return true;
        }
    }

    class DemoPropertyAspectAttribute : Core.Attribute.MPAspectAttribute
    {
        public override void OnEntry(object sender, Core.Attribute.AspectEventArgs args)
        {
            args.MethodName = "OnEntry " + args.MethodName + "  " + args.Params;
            Form1.OnEntry?.Invoke(sender, args);
        }

        public override void OnExit(object sender, Core.Attribute.AspectEventArgs args)
        {
            args.MethodName = "OnExit " + args.MethodName + "  " + args.Params;
            Form1.OnExit?.Invoke(sender, args);
        }

        static int aaa = 1;
        public override bool Validating(object sender, Core.Attribute.AspectEventArgs args)
        {
            return (aaa++) % 2 == 0;
        }
    }
}

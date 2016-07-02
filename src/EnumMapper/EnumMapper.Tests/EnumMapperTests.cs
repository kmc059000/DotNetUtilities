using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnumMapper.Tests
{
    [TestClass]
    public class EnumMapperTests
    {
        [TestMethod]
        public void EnumMapper_MapsEnums()
        {
            var enumMapper = new E1E2EnumMapper();

            Assert.AreEqual(E2.A, enumMapper.Map(E1.A));
            Assert.AreEqual(E2.B, enumMapper.Map(E1.B));
            Assert.AreEqual(E2.C, enumMapper.Map(E1.C));
            Assert.AreEqual(E2.D, enumMapper.Map(E1.D));

            Assert.AreEqual(E1.A, enumMapper.Map(E2.A));
            Assert.AreEqual(E1.B, enumMapper.Map(E2.B));
            Assert.AreEqual(E1.C, enumMapper.Map(E2.C));
            Assert.AreEqual(E1.D, enumMapper.Map(E2.D));
        }

        [TestMethod]
        public void EnumMapperAutoMap_DoesNotMapEnums_IfMappingDoesntExist_MapsEnums()
        {
            var enumMapper = new E1E2EnumMapper();

            ThrowsException(() => enumMapper.Map(E1.E));
            ThrowsException(() => enumMapper.Map(E1.F));
            ThrowsException(() => enumMapper.Map(E1.G));
            ThrowsException(() => enumMapper.Map(E1.H));

            ThrowsException(() => enumMapper.Map(E2.X));
            ThrowsException(() => enumMapper.Map(E2.Y));
            ThrowsException(() => enumMapper.Map(E2.Z));

            Assert.AreEqual(null, enumMapper.TryMap((E1?)null));
            Assert.AreEqual(null, enumMapper.TryMap(E1.E));
            Assert.AreEqual(null, enumMapper.TryMap(E1.F));
            Assert.AreEqual(null, enumMapper.TryMap(E1.G));
            Assert.AreEqual(null, enumMapper.TryMap(E1.H));

            Assert.AreEqual(null, enumMapper.TryMap((E2?)null));
            Assert.AreEqual(null, enumMapper.TryMap(E2.X));
            Assert.AreEqual(null, enumMapper.TryMap(E2.Y));
            Assert.AreEqual(null, enumMapper.TryMap(E2.Z));

        }

        [TestMethod]
        public void EnumMapperAutoMap_MapsEnums()
        {
            var enumMapper = new E1E3EnumAutoMapper();

            Assert.AreEqual(E3.E, enumMapper.Map(E1.E));
            Assert.AreEqual(E3.F, enumMapper.Map(E1.F));
            Assert.AreEqual(E3.G, enumMapper.Map(E1.G));
            Assert.AreEqual(E3.H, enumMapper.Map(E1.H));

            Assert.AreEqual(E1.E, enumMapper.Map(E3.E));
            Assert.AreEqual(E1.F, enumMapper.Map(E3.F));
            Assert.AreEqual(E1.G, enumMapper.Map(E3.G));
            Assert.AreEqual(E1.H, enumMapper.Map(E3.H));
        }

        [TestMethod]
        public void EnumMapper_DoesNotMapEnums_IfMappingDoesntExist_MapsEnums()
        {
            var enumMapper = new E1E3EnumAutoMapper();

            ThrowsException(() => enumMapper.Map(E1.A));
            ThrowsException(() => enumMapper.Map(E1.B));
            ThrowsException(() => enumMapper.Map(E1.C));
            ThrowsException(() => enumMapper.Map(E1.D));

            ThrowsException(() => enumMapper.Map(E3.X));
            ThrowsException(() => enumMapper.Map(E3.Y));
            ThrowsException(() => enumMapper.Map(E3.Z));

            Assert.AreEqual(null, enumMapper.TryMap((E1?)null));
            Assert.AreEqual(null, enumMapper.TryMap(E1.A));
            Assert.AreEqual(null, enumMapper.TryMap(E1.B));
            Assert.AreEqual(null, enumMapper.TryMap(E1.C));
            Assert.AreEqual(null, enumMapper.TryMap(E1.D));

            Assert.AreEqual(null, enumMapper.TryMap((E3?)null));
            Assert.AreEqual(null, enumMapper.TryMap(E3.X));
            Assert.AreEqual(null, enumMapper.TryMap(E3.Y));
            Assert.AreEqual(null, enumMapper.TryMap(E3.Z));
        }

        private void ThrowsException<T>(Func<T> func)
        {
            var thrown = false;
            try
            {
                func();
            }
            catch
            {
                thrown = true;
            }

            if (!thrown)
            {
                Assert.Fail("Exception was not thrown");
            }
        }

        public class E1E2EnumMapper : EnumMapper<E1, E2>
        {
            static E1E2EnumMapper()
            {
                CreateMap(E1.A, E2.A);
                CreateMap(E1.B, E2.B);
                CreateMap(E1.C, E2.C);
                CreateMap(E1.D, E2.D);
            }
        }

        public class E1E3EnumAutoMapper : EnumMapper<E1, E3>
        {
            static E1E3EnumAutoMapper()
            {
                AutoMap();
            }
        }


        public enum E1
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        public enum E2
        {
            A,
            B,
            C,
            D,

            X,
            Y,
            Z
        }

        public enum E3
        {
            E,
            F,
            G,
            H,

            X,
            Y,
            Z
        }

    }
}

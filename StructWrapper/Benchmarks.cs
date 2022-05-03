using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StructWrapper
{
    public class Benchmarks
    {
        private static int iterationCount = 1_000;

        private List<int> sampleIntValues;
        private List<float> sampleFloatValues;
        private List<double> sampleDoubleValues;
        private byte[] targetBuffer;

        public Benchmarks()
        {
            sampleIntValues = new List<int>(iterationCount * 10);
            sampleFloatValues = new List<float>(iterationCount * 10);
            sampleDoubleValues = new List<double>(iterationCount * 10);
            targetBuffer = new byte[iterationCount * 10 * sizeof(double)];

            var rand = new Random();
            for (int i = 0; i < iterationCount * 10; i++)
            {
                sampleIntValues.Add(rand.Next());
                sampleFloatValues.Add(rand.NextSingle());
                sampleDoubleValues.Add(rand.NextDouble());
            }
        }

        #region Simple Span Case. All methods are directly accessing the span
        public readonly struct MethodHost_Span
        {
            public MethodHost_Span WriteInt32_Span(ref Span<byte> span, int value)
            {
                MemoryMarshal.Cast<byte, int>(span)[0] = value;
                span = span.Slice(sizeof(int));
                return default(MethodHost_Span);
            }

            public MethodHost_Span WriteSingle_Span(ref Span<byte> span, float value)
            {
                MemoryMarshal.Cast<byte, float>(span)[0] = value;
                span = span.Slice(sizeof(float));
                return default(MethodHost_Span);
            }

            public MethodHost_Span WriteDouble_Span(ref Span<byte> span, double value)
            {
                MemoryMarshal.Cast<byte, double>(span)[0] = value;
                span = span.Slice(sizeof(double));
                return default(MethodHost_Span);
            }
        }
        #endregion

        #region The case where Span is wrapped in an otherwise empty struct. All methods are accessing the span through the wrapper.
        public ref struct WrappedSpan
        {
            public Span<byte> Span;
        }

        public readonly struct MethodHost_WrappedSpan
        {
            public MethodHost_WrappedSpan WriteInt32_WrappedSpan(ref WrappedSpan wrapper, int value)
            {
                MemoryMarshal.Cast<byte, int>(wrapper.Span)[0] = value;
                wrapper.Span = wrapper.Span.Slice(sizeof(int));
                return default(MethodHost_WrappedSpan);
            }

            public MethodHost_WrappedSpan WriteSingle_WrappedSpan(ref WrappedSpan wrapper, float value)
            {
                MemoryMarshal.Cast<byte, float>(wrapper.Span)[0] = value;
                wrapper.Span = wrapper.Span.Slice(sizeof(float));
                return default(MethodHost_WrappedSpan);
            }

            public MethodHost_WrappedSpan WriteDouble_WrappedSpan(ref WrappedSpan wrapper, double value)
            {
                MemoryMarshal.Cast<byte, double>(wrapper.Span)[0] = value;
                wrapper.Span = wrapper.Span.Slice(sizeof(double));
                return default(MethodHost_WrappedSpan);
            }
        }
        #endregion

        [Benchmark]
        public void WriteMany_Int32_Span()
        {
            Span<byte> span = targetBuffer;

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_Span)
                    .WriteInt32_Span(ref span, sampleIntValues[i])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 1])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 2])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 3])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 4])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 5])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 6])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 7])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 8])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Int32_WrappedSpan()
        {
            WrappedSpan wrappedSpan = new WrappedSpan()
            {
                Span = targetBuffer
            };

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_WrappedSpan)
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 1])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 2])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 3])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 4])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 5])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 6])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 7])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 8])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Single_Span()
        {
            Span<byte> span = targetBuffer;

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_Span)
                    .WriteSingle_Span(ref span, sampleFloatValues[i])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 1])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 2])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 3])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 4])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 5])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 6])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 7])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 8])
                    .WriteSingle_Span(ref span, sampleFloatValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Single_WrappedSpan()
        {
            WrappedSpan wrappedSpan = new WrappedSpan()
            {
                Span = targetBuffer
            };

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_WrappedSpan)
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 1])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 2])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 3])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 4])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 5])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 6])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 7])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 8])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleFloatValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Double_Span()
        {
            Span<byte> span = targetBuffer;

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_Span)
                    .WriteDouble_Span(ref span, sampleDoubleValues[i])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 1])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 2])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 3])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 4])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 5])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 6])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 7])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 8])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Double_WrappedSpan()
        {
            WrappedSpan wrappedSpan = new WrappedSpan()
            {
                Span = targetBuffer
            };

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_WrappedSpan)
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 1])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 2])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 3])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 4])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 5])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 6])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 7])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 8])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Mixed_Span()
        {
            Span<byte> span = targetBuffer;

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_Span)
                    .WriteInt32_Span(ref span, sampleIntValues[i])
                    .WriteSingle_Span(ref span, sampleIntValues[i + 1])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 2])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 3])
                    .WriteSingle_Span(ref span, sampleIntValues[i + 4])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 5])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 6])
                    .WriteSingle_Span(ref span, sampleIntValues[i + 7])
                    .WriteDouble_Span(ref span, sampleDoubleValues[i + 8])
                    .WriteInt32_Span(ref span, sampleIntValues[i + 9]);
            }
        }

        [Benchmark]
        public void WriteMany_Mixed_WrappedSpan()
        {
            WrappedSpan wrappedSpan = new WrappedSpan()
            {
                Span = targetBuffer
            };

            for (int i = 0; i < iterationCount; i += 10)
            {
                default(MethodHost_WrappedSpan)
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 1])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 2])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 3])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 4])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 5])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 6])
                    .WriteSingle_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 7])
                    .WriteDouble_WrappedSpan(ref wrappedSpan, sampleDoubleValues[i + 8])
                    .WriteInt32_WrappedSpan(ref wrappedSpan, sampleIntValues[i + 9]);
            }
        }
    }
}

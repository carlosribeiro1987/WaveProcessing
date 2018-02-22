using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProcessing {
    public class Wave {
        private int _length;
        private int _dataLength;
        private int _bitPerSample;
        private int _sampleRate;
        private short _channels;
        private ushort _maxLevel;
        private byte[] _data;

        public enum WaveFileMode { Read, Write, ReadWrite };


        public Wave() {
        }
        public Wave(string filePath) {

        }

        public bool ReadWaveHeader(string inputPath) {
            if (string.IsNullOrWhiteSpace(inputPath)) {
                throw new WaveException("The input file path cannot be empty.");
            }
            if (Path.GetExtension(inputPath).ToLower() != ".wav".ToLower()) {
                throw new WaveException(string.Format(@"The file '{0}' is not a *.wav file.", inputPath));
            }

            FileStream stream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            try {
                _length = (int)stream.Length - 8;
                stream.Position = 22; //channel
                _channels = reader.ReadInt16();
                stream.Position = 24; //sample rate
                _sampleRate = reader.ReadInt32();
                stream.Position = 34; //bits per sample
                _bitPerSample = reader.ReadInt16();
                _dataLength = (int)stream.Length - 44;
                _data = new byte[stream.Length - 44];
                stream.Position = 44;
                stream.Read(_data, 0, _data.Length);
            }
            catch {
                return false;
            }
            finally {
                reader.Close();
                stream.Close();
            }
            return true;
        }

        public bool WriteWaveHeader(string filename, string outputDir) {
            if (string.IsNullOrWhiteSpace(outputDir)) {
                throw new WaveException("The output directory path cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(filename)) {
                throw new WaveException("Output file name cannot be empty.");
            }

            if (Path.GetExtension(filename).ToLower() != ".wav" || Path.GetExtension(filename).ToLower() != ".wave") {
                filename = Path.GetFileNameWithoutExtension(filename) + ".wav";
            }
            string filePath = Path.Combine(outputDir, filename);
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            try {
                stream.Position = 0;
                writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
                writer.Write(_length);
                writer.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                writer.Write((int)16);
                writer.Write((short)1);
                writer.Write(_channels);
                writer.Write(_sampleRate);
                writer.Write((int)(_sampleRate * ((_bitPerSample * _channels) / 8)));
                writer.Write((short)((_bitPerSample * _channels) / 8));
                writer.Write(_bitPerSample);
                writer.Write(new char[4] { 'd', 'a', 't', 'a' });
                writer.Write(_dataLength);
            }
            catch {
                return false;
            }
            finally {
                writer.Close();
                stream.Close();
            }
            return true;
        }

        public void WriteWaveData(string wavPath, ref byte[] data) {
            try {
                FileStream stream = new FileStream(wavPath, FileMode.Append, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(data);
                writer.Close();
                stream.Close();
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public int Length {
            get { return _length; }
        }
        public int DataLength {
            get { return _dataLength; }
        }
        public int BitsPerSample {
            get { return _bitPerSample; }
        }
        public byte[] WaveData {
            get { return _data; }
        }
    }
}

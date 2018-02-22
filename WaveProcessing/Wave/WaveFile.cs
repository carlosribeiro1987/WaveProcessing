using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProcessing.Wave {
    class WaveFile {
        private string _fileName;
        private FileStream _fileStream;
        private char[] waveHeader;
        private char[] riffType;
        private byte _numChannels;
        private int _dataSize; //data size in bytes
        private int _sampleRemaining;
        private int _sampleRate; //sample rate in Hz
        private int _bitsSample; //bits per sample
        private short _bitsSec; //bits per second
        private short _bytesSec; //bytes per second
        private static short _dataStartPos;
        private WaveFileMode _fileMode;


        public enum WaveFileMode { Read, Write, ReadWrite };

        public string Open(WaveFileMode mode) {
            if (_fileStream != null) {
                _fileStream.Close();
                _fileStream.Dispose();
                _fileStream = null;
            }
            string filenameBackup = _fileName;
            InitMembers();


            if (mode != WaveFileMode.Read || mode != WaveFileMode.Write)
                throw new WaveFileException("File mode is not suported: " + mode.ToString());
        }

        public string Open(string file, WaveFileMode mode) {
            return Open(mode);
        }
    }
}

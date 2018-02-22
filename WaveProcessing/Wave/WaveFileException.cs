using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProcessing.Wave {
    class WaveFileException : System.Exception {
        public WaveFileException(string errorMessage) : base(errorMessage) {

        }
    }
}

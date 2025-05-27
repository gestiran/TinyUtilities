using System;

namespace TinyUtilities {
    public static class TextUtility {
        private const int _K = 1000;
        private const int _K2 = 10000;
        private const int _M = 1000000;
        private const int _M2 = 10000000;
        
        public static string ConvertWithAbbreviation(int count) {
            if (count < _K) {
                return $"{count}";
            }
            
            if (count < _K2) {
                return $"{(float)count / _K:0.'<size=80%>'0}<size=100%> K";
            }
            
            if (count < _M) {
                return $"{count / _K} K";
            }
            
            if (count < _M2) {
                return $"{(float)count / _M:0.'<size=80%>'0}<size=100%> M";
            }
            
            return $"{count / _M} M";
        }
        
        public static string ConvertWithAbbreviation(float count) {
            if (count < _K) {
                return $"{count:##.#}";
            }
            
            if (count < _K2) {
                return $"{count / _K:0.'<size=80%>'0}<size=100%> K";
            }
            
            if (count < _M) {
                return $"{count / _K:##.#} K";
            }
            
            if (count < _M2) {
                return $"{count / _M:0.'<size=80%>'0}<size=100%> M";
            }
            
            return $"{count / _M:##.#} M";
        }
        
        public static string TimeHMS(TimeSpan time) => $"{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}";
    }
}
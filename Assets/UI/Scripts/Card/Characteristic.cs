using System;

namespace UI.Scripts.Card
{
    public interface Characteristic<out T> where T : IComparable 
    {
        public T GetValue();

        public void ApplyConstraint();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class Transform: Component, IUpdateable
    {
    public Vector3 localPosition;
    private Quaternion localRotation;
    private Vector3 localScale;
    private Matrix world;
    private Transform parent;
    //List with children of this transform

    private List<Transform> Children { get; set; }

    public Vector3 LocalPosition
    {
      get { return localPosition; }
      set { localPosition = value; UpdateWorld(); }
    }


    public Vector3 LocalScale
    {
    get { return localScale; }
    set { localScale = value; UpdateWorld(); }
    }

    public Quaternion LocalRotation
    {
    get { return localRotation; }
    set { localRotation = value; UpdateWorld(); }
    }

        public Matrix World { get { return world; } }
        public Vector3 Forward { get { return world.Forward; } }
        public Vector3 Backward { get { return world.Backward; } }
        public Vector3 Right { get { return world.Right; } }
        public Vector3 Left { get { return world.Left; } }
        public Vector3 Up { get { return world.Up; } }
        public Vector3 Down { get { return world.Down; } }



        //*** methods ***

        public Transform()
        {
            Children = new List<Transform>();
            parent = null;
            localPosition = Vector3.Zero;
            localRotation = Quaternion.Identity;
            localScale = Vector3.One;
            UpdateWorld();
        }

        public Transform Parent
        {
            get { return parent; }
            set
            {
                // Failsafe check - do not allow self-loops
                // But what about non-self loops?
                if (value == this) return;
                // If there is a parent right now, remove me from that list
                if (parent != null)
                    parent.Children.Remove(this);
                parent = value;
                // If I do have a parent, add me me to it
                if (parent != null)
                    parent.Children.Add(this);
                UpdateWorld();
            }
        }

        private void UpdateWorld()
        {
            world = Matrix.CreateScale(localScale) *
                Matrix.CreateFromQuaternion(localRotation) *
                Matrix.CreateTranslation(localPosition);
            if (parent != null)
                world = world * parent.World;
            foreach (Transform child in Children)
                child.UpdateWorld();
        }

        public Vector3 Position
        {
            get { return world.Translation; }
            set
            {
                if (Parent == null)
                    LocalPosition = value;
                else
                {
                    Matrix total = World;
                    total.Translation = value;
                    LocalPosition = (Matrix.Invert(Matrix.CreateScale(LocalScale) * Matrix.CreateFromQuaternion(LocalRotation)) *
                    total * Matrix.Invert(Parent.World)).Translation;
                }
            }
        }

        public Quaternion Rotation
        {
            get { return Quaternion.CreateFromRotationMatrix(World); }
            set
            {
                if (Parent == null)
                    LocalRotation = value;
                else
                {
                    Vector3 scale, pos; Quaternion rot;
                    world.Decompose(out scale, out rot, out pos);
                    Matrix total = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(value) 
                        * Matrix.CreateTranslation(pos);
                    LocalRotation = Quaternion.CreateFromRotationMatrix(Matrix.Invert(Matrix.CreateScale(LocalScale)) 
                        * total * Matrix.Invert(Matrix.CreateTranslation(LocalPosition) * Parent.world));
                }
            }
        }



        public Vector3 Scale
        {
            get
            {
                Vector3 s, t; Quaternion r;
                world.Decompose(out s, out r, out t);
                return s;
            }
        }



        public void Rotate(Vector3 axis, float angle)
        {
            LocalRotation *=
                Quaternion.CreateFromAxisAngle(axis, angle);
        }

        public void Update()
        {
            //throw new NotImplementedException();
            UpdateWorld();
        }
    }
}

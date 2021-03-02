using Godot;

namespace Entities.Nodes
{
    public class PlayerFPS : KinematicBody
    {
        private Spatial _head;
        private Camera _view;
        private float _cameraAngle = 0f;
        private float _mouseSensitivity = 0.1f;
        private float _moveSpeed = 30f;
        private float _moveAccel = 8f;
        private Vector3 _velocity;

        public override void _Ready()
        {
            Input.SetMouseMode(Input.MouseMode.Captured);
            _head = GetNode<Spatial>("Head");
            _view = GetNode<Camera>("Head/View");
        }

        public override void _PhysicsProcess(float delta)
        {
            Walk(delta);
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion)
            {
                InputEventMouseMotion motion = (InputEventMouseMotion) @event;
                _head.RotateY(Mathf.Deg2Rad(-motion.Relative.x * _mouseSensitivity));
                float change = -motion.Relative.y * _mouseSensitivity;
                
                if ((change + _cameraAngle) < 90f && (change + _cameraAngle) > -90f)
                {
                    _view.RotateX(Mathf.Deg2Rad(change));
                    _cameraAngle += change;
                }
            }
            if (@event is InputEventKey)
            {
                InputEventKey key = (InputEventKey) @event;

                if (key.IsAction("ui_cancel"))
                {
                    Input.SetMouseMode(Input.MouseMode.Visible);
                }
            }
        }

        private void Walk(float delta)
        {
            Vector3 direction = new Vector3();
            Basis aim = _view.GlobalTransform.basis;

            if (Input.IsActionPressed("ui_up"))
            {
                direction -= aim.z;
            }
            if (Input.IsActionPressed("ui_down"))
            {
                direction += aim.z;
            }
            if (Input.IsActionPressed("ui_left"))
            {
                direction -= aim.x;
            }
            if (Input.IsActionPressed("ui_right"))
            {
                direction += aim.x;
            }
            _velocity = _velocity.LinearInterpolate(
                direction.Normalized() * _moveSpeed,
                _moveAccel * delta
            );
            MoveAndSlide(_velocity);
        }
    }
}

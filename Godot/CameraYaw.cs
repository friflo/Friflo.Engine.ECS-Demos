using Godot;

public partial class CameraYaw : Node3D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        RotateObjectLocal(Vector3.Up, 0.5f * (float)delta);
	}
}

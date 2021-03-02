using Godot;

public class World : Spatial
{
    private RID _mesh;
    private RID _instance;
    private RID _shape;
    private RID _body;

    public override void _Ready()
    {
        AddMesh();
        AddCollision();
    }

    private void AddMesh()
    {
        Vector3[] vertices = new Vector3[] {
            new Vector3(-5f, 0f, -5f),
            new Vector3(5f, 0f, -5f),
            new Vector3(5f, 0f, 5f),
            new Vector3(-5f, 0f, 5f)
        };
        Vector3[] normals = new Vector3[] {
            Vector3.Up,
            Vector3.Up,
            Vector3.Up,
            Vector3.Up
        };
        int[] indices = new int[] {0, 1, 2, 0, 2, 3};

        Godot.Collections.Array meshData = new Godot.Collections.Array();
        meshData.Resize((int) Mesh.ArrayType.Max);
        meshData[(int) Mesh.ArrayType.Vertex] = vertices;
        meshData[(int) Mesh.ArrayType.Normal] = normals;
        meshData[(int) Mesh.ArrayType.Index] = indices;

        _mesh = VisualServer.MeshCreate();
        VisualServer.MeshAddSurfaceFromArrays(_mesh, VisualServer.PrimitiveType.Triangles, meshData);

        _instance = VisualServer.InstanceCreate();
        VisualServer.InstanceSetBase(_instance, _mesh);

        RID scenario = GetWorld().Scenario;
        VisualServer.InstanceSetScenario(_instance, scenario);
    }

    private void AddCollision()
    {
        Vector3[] vertices = new Vector3[] {
            new Vector3(-5f, 0f, -5f),
            new Vector3(5f, 0f, -5f),
            new Vector3(5f, 0f, 5f),
            new Vector3(-5f, 0f, -5f),
            new Vector3(5f, 0f, 5f),
            new Vector3(-5f, 0f, 5f)
        };

        _shape = PhysicsServer.ShapeCreate(PhysicsServer.ShapeType.ConcavePolygon);
        PhysicsServer.ShapeSetData(_shape, vertices);

        _body = PhysicsServer.BodyCreate(PhysicsServer.BodyMode.Static);
        PhysicsServer.BodyAddShape(_body, _shape);

        RID space = GetWorld().Space;
        PhysicsServer.BodySetSpace(_body, space);
    }
}

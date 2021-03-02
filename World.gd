extends Spatial

var _mesh: RID
var _instance: RID
var _shape: RID
var _body: RID

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	add_mesh()
	add_collision()

func add_mesh() -> void:
	var vertices := PoolVector3Array([
		Vector3(-5, 0, -5),
		Vector3(5, 0, -5),
		Vector3(5, 0, 5),
		Vector3(-5, 0, 5),
	])
	var normals := PoolVector3Array([
		Vector3.UP,
		Vector3.UP,
		Vector3.UP,
		Vector3.UP
	])
	var indices := PoolIntArray([0, 1, 2, 0, 2, 3])
	
	var mesh_data := []
	mesh_data.resize(Mesh.ARRAY_MAX)
	mesh_data[Mesh.ARRAY_VERTEX] = vertices
	mesh_data[Mesh.ARRAY_NORMAL] = normals
	mesh_data[Mesh.ARRAY_INDEX] = indices
	
	_mesh = VisualServer.mesh_create()
	VisualServer.mesh_add_surface_from_arrays(_mesh, VisualServer.PRIMITIVE_TRIANGLES, mesh_data)
	
	_instance = VisualServer.instance_create()
	VisualServer.instance_set_base(_instance, _mesh)
	
	var scenario: RID = get_world().scenario
	VisualServer.instance_set_scenario(_instance, scenario)

# works in GDScript with no problem
func add_collision() -> void:
	var vertices := PoolVector3Array([
		Vector3(-5, 0, -5),
		Vector3(5, 0, -5),
		Vector3(5, 0, 5),
		Vector3(-5, 0, -5),
		Vector3(5, 0, 5),
		Vector3(-5, 0, 5)
	])
	_shape = PhysicsServer.shape_create(PhysicsServer.SHAPE_CONCAVE_POLYGON)
	PhysicsServer.shape_set_data(_shape, vertices)
	
	_body = PhysicsServer.body_create(PhysicsServer.BODY_MODE_STATIC)
	PhysicsServer.body_add_shape(_body, _shape)
	
	var space: RID = get_world().space
	PhysicsServer.body_set_space(_body, space)

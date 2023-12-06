<?php
session_start();
$ci = $_SESSION["ci"];
$link = mysqli_connect("localhost", "infu324", "123456", "workflow");

// Verificar si se ha enviado el formulario de elección de materia
if ($_SERVER["REQUEST_METHOD"] == "GET" && isset($_GET["elegirMateria"])) {
    $materiaSeleccionada = mysqli_real_escape_string($link, $_GET["materiaSeleccionada"]);

    // Obtener información sobre la materia seleccionada
    $resultMateria = mysqli_query($link, "SELECT * FROM academico.materias WHERE codigo='$materiaSeleccionada'");
    $filaMateria = mysqli_fetch_array($resultMateria);

    // Verificar si hay espacio disponible para inscribirse en la materia
    if ($filaMateria["numero_inscritos"] < $filaMateria["maximo_inscritos"]) {
        // Actualizar la columna materia_elegida en la tabla academico.alumno
        $actualizarMateria = "UPDATE academico.alumno SET materia_elegida='$materiaSeleccionada' WHERE ci='$ci'";
        mysqli_query($link, $actualizarMateria);

        // Incrementar el contador de inscritos en la materia
        $nuevoInscritos = $filaMateria["numero_inscritos"] + 1;
        mysqli_query($link, "UPDATE academico.materias SET numero_inscritos=$nuevoInscritos WHERE codigo='$materiaSeleccionada'");
        echo "MATERIA ELEGIDA";
        header("Location: motor.php?flujo=F1&procesosiguiente=P6");
        exit();
    } else {
        // Si no hay espacio disponible, mostrar un mensaje
        $mensaje = "No hay espacio disponible en la materia seleccionada.";
    }
}

// Obtener la lista de materias disponibles
$resultMaterias = mysqli_query($link, "SELECT codigo FROM academico.materias");
$materiasDisponibles = mysqli_fetch_all($resultMaterias, MYSQLI_ASSOC);

// Obtener información sobre el estado de verificación de documentos
$resultado = mysqli_query($link, "SELECT * FROM academico.alumno WHERE ci='$ci'");
$datosAlumno = mysqli_fetch_array($resultado);
$actualizado = $datosAlumno["Docuactualizado"];
?>

<?php if ($actualizado == 1): ?>
    USUARIO: <?php echo $datosAlumno["nombre"] ?> CI: <?php echo $ci ?><br>
    <?php
    // Obtener información sobre la materia ya elegida
    $resultAlumno = mysqli_query($link, "SELECT * FROM academico.alumno WHERE ci='$ci'");
    $filaAlumno = mysqli_fetch_array($resultAlumno);

    // Verificar si ya hay una materia elegida
    if ($filaAlumno["materia_elegida"] !== null): ?>
        <p>Ya has elegido una materia.</p>
    <?php else: ?>
    <form action="correcto.vista.inc.php" method="GET">
        <label for="materiaSeleccionada">ELEGIR MATERIA:</label>
        <select name="materiaSeleccionada">
            <?php foreach ($materiasDisponibles as $materia): ?>
                <option value="<?php echo $materia["codigo"]; ?>"><?php echo $materia["codigo"]; ?></option>
            <?php endforeach; ?>
        </select>
        <input type="hidden" name="elegirMateria" value="1">
        <input type="hidden" name="flujo" value="<?php echo $_GET["flujo"]; ?>">
        <input type="hidden" name="proceso" value="<?php echo $_GET["proceso"]; ?>">
        <input type="submit" value="Aceptar">
    </form>
    <?php endif; ?>
    <?php if (isset($mensaje)): ?>
        <p><?php echo $mensaje; ?></p>
    <?php endif; ?>

<?php else: ?>
    <!-- Aquí va el contenido para el caso contrario -->
    <!-- Puedes mostrar un mensaje, contenido adicional, etc. -->
    <p>Los documentos no estan verificados.</p>
<?php endif; ?>

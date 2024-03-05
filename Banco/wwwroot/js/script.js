function revisarContraseña() {
    const contraseña = document.getElementById('ClaveUsuario').value;
    const confContraseña = document.getElementById('confirmarPassword').value;
    const mensajeContraseña = document.getElementById('mensajeContrasenya');
    const contraseñaRegex = /^(?=.*\d).{8,}$/;

    if (contraseña === "" && confContraseña === "") {
        mensajeContraseña.innerHTML = '<span class="badge bg-danger">No puede dejar los campos contraseñas vacíos</span>';
        mensajeContraseña.style.color = 'red';
        document.getElementById("btnRegistro").disabled = true;
        btnRegistro.style.backgroundColor = "#959595";
    } else if (contraseña === confContraseña) {
        if (contraseñaRegex.test(contraseña)) {
            mensajeContraseña.innerHTML = '<span class="badge bg-success">Contraseña válida</span>';
            mensajeContraseña.style.color = 'green';
            document.getElementById("btnRegistro").disabled = false;
            btnRegistro.style.backgroundColor = "#5993d3";
        } else {
            mensajeContraseña.innerHTML = '<span class="badge bg-danger">La contraseña debe tener al menos 8 caracteres con 1 número</span>';
            mensajeContraseña.style.color = 'red';
            document.getElementById("btnRegistro").disabled = true;
            btnRegistro.style.backgroundColor = "#959595";
        }
    } else {
        mensajeContraseña.innerHTML = '<span class="badge bg-danger">Las contraseñas introducidas no son iguales</span>';
        mensajeContraseña.style.color = 'red';
        document.getElementById("btnRegistro").disabled = true;
        btnRegistro.style.backgroundColor = "#959595";
    }
}


function mostrarNotificacion(titulo, mensaje, tipo) {
    Swal.fire({
        title: titulo,
        text: mensaje,
        icon: tipo,
        confirmButtonText: 'OK',
        customClass: {
            confirmButton: 'btn btn-primary'
        }
    });
}

function confirmarLogout() {
    Swal.fire({
        title: '¿Estás seguro de que deseas cerrar sesión?',
        text: 'Serás redirigido a la página de bienvenida.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, cerrar sesión'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById('logoutForm').submit();
        } else {
            console.log('Logout cancelado');
        }
    });
}
function confirmar(mensaje) {
    return Swal.fire({
        title: '¿Estás seguro de que deseas ' + mensaje + '?',
        text: 'Esta acción es irreversible.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí',
        cancelButtonText: 'No'
    }).then((result) => {
        return result.isConfirmed;
    });
}


function confirmarEliminar(event) {
    const idUsuario = event.currentTarget.getAttribute("data-id");
    confirmar("eliminar el usuario").then(function (confirmado) {
        if (confirmado) {
            window.location.href = 'https://localhost:7142/privada/eliminar-usuario/' + idUsuario;
        }
    });
}
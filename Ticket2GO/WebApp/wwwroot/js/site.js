function togglePasswordVisibility() {
    let passwordInput = document.getElementById('Input_Password');
    let showPasswordCheckbox = document.getElementById('showPassword');

    if (showPasswordCheckbox.checked) {
        passwordInput.type = 'text';
    } else {
        passwordInput.type = 'password';
    }
}

function toggleConfirmPasswordVisibility() {
    let confirmPasswordInput = document.getElementById('Input_ConfirmPassword');
    let showConfirmPasswordCheckbox = document.getElementById('showConfirmPassword');

    if (showConfirmPasswordCheckbox.checked) {
        confirmPasswordInput.type = 'text';
    } else {
        confirmPasswordInput.type = 'password';
    }
}

function passwordStrength() {
    const passwordInput = document.getElementById('Input_Password');
    const passwordStrengthBar = document.getElementById('passwordStrengthBar');
    const password = passwordInput.value;
    let strength = 0;

    if (password.match(/[a-z]+/)) strength += 1;
    if (password.match(/[A-Z]+/)) strength += 1;
    if (password.match(/[0-9]+/)) strength += 1;
    if (password.match(/[\W_]+/)) strength += 1;

    switch (strength) {
        case 0:
            passwordStrengthBar.style.width = '0%';
            passwordStrengthBar.classList.remove('bg-danger', 'bg-warning', 'bg-info', 'bg-success');
            break;
        case 1:
            passwordStrengthBar.style.width = '25%';
            passwordStrengthBar.classList.add('bg-danger');
            passwordStrengthBar.classList.remove('bg-warning', 'bg-info', 'bg-success');
            break;
        case 2:
            passwordStrengthBar.style.width = '50%';
            passwordStrengthBar.classList.add('bg-warning');
            passwordStrengthBar.classList.remove('bg-danger', 'bg-info', 'bg-success');
            break;
        case 3:
            passwordStrengthBar.style.width = '75%';
            passwordStrengthBar.classList.add('bg-info');
            passwordStrengthBar.classList.remove('bg-danger', 'bg-warning', 'bg-success');
            break;
        case 4:
            passwordStrengthBar.style.width = '100%';
            passwordStrengthBar.classList.add('bg-success');
            passwordStrengthBar.classList.remove('bg-danger', 'bg-warning', 'bg-info');
            break;
    }
}
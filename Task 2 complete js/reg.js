// validation for first name 
function validateFirstName() {
    const firstNameInput = document.getElementById('firstName');
    const errorSpan = document.getElementById('firstNameError');
    const submitButton = document.querySelector('.submit-button');
    const value = firstNameInput.value;

    // Regular expression to check for letters only (no spaces allowed)
    const regex = /^[A-Za-z]+$/;

    let isValid = true;

    if (value !== value.trim()) {
        errorSpan.textContent = 'No spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value === '') {
        errorSpan.textContent = 'First Name cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Only letters and no spaces.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'At least 3 letters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 15) {
        errorSpan.textContent = 'Maximum 15 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    if (isValid) {
        firstNameInput.classList.remove('error-border');
        firstNameInput.classList.add('valid-border');
    } else {
        firstNameInput.classList.remove('valid-border');
        firstNameInput.classList.add('error-border');
    }

    submitButton.disabled = !isValid;
}

// valiadtion for last name 
function validateLastName() {
    const lastNameInput = document.getElementById('lastName');
    const errorSpan = document.getElementById('lastNameError');
    const submitButton = document.querySelector('.submit-button');
    const value = lastNameInput.value;

    // Regular expression to check for valid characters (letters only, no spaces)
    const regex = /^[A-Za-z]+$/;

    let isValid = true;

    if (value !== value.trim()) {
        errorSpan.textContent = 'No spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value === '') {
        errorSpan.textContent = 'Last Name cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Only letters and no spaces.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'At least 3 letters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 10) {
        errorSpan.textContent = 'Maximum 10 letters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    // Add or remove error-border class
    if (isValid) {
        lastNameInput.classList.remove('error-border');
        lastNameInput.classList.add('valid-border');
    } else {
        lastNameInput.classList.remove('valid-border');
        lastNameInput.classList.add('error-border');
    }


    submitButton.disabled = !isValid;
}

// validation for phone numbers 
function validatePhoneNumber() {
    const phoneNumberInput = document.getElementById('phoneNumber');
    const errorSpan = document.getElementById('phoneNumberError');
    const submitButton = document.querySelector('.submit-button');
    let value = phoneNumberInput.value;

    // Regular expression to check if the input contains only digits with no spaces
    const regex = /^[0-9]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Phone Number cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.includes(' ')) {
        errorSpan.textContent = 'Spaces are not allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Only numbers are allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!['6', '7', '8', '9'].includes(value[0])) {
        errorSpan.textContent = 'Must start with 6, 7, 8, or 9.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length !== 10) {
        errorSpan.textContent = 'Must be 10 digits.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    // Add or remove error-border class
    if (isValid) {
        phoneNumberInput.classList.remove('error-border');
        phoneNumberInput.classList.add('valid-border');
    } else {
        phoneNumberInput.classList.remove('valid-border');
        phoneNumberInput.classList.add('error-border');
    }

    submitButton.disabled = !isValid;
}

// validation for email 
function validateEmail() {
    const emailInput = document.getElementById('email');
    const errorSpan = document.getElementById('emailError');
    const submitButton = document.querySelector('.submit-button');
    const value = emailInput.value;

    // Regular expression to validate email format
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Email cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Enter a valid email address.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

// Add or remove border styles based on validity
if (isValid) {
    emailInput.classList.remove('error-border');
    emailInput.classList.add('valid-border');
} else {
    emailInput.classList.remove('valid-border');
    emailInput.classList.add('error-border');
}


    submitButton.disabled = !isValid;
}

// validation for street address 

function validateStreetAddress() {
    const streetAddressInput = document.getElementById('streetAddress');
    const errorSpan = document.getElementById('streetAddressError');
    const submitButton = document.querySelector('.submit-button');
    const value = streetAddressInput.value;

    // Regular expression to allow letters, numbers, spaces, commas, periods, hyphens, and apostrophes
    const regex = /^[A-Za-z0-9\s,.'-]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Street Address cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Street Address can only contain letters, numbers, spaces, commas, periods, hyphens, and apostrophes.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'Must be at least 3 characters long.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 50) {
        errorSpan.textContent = 'Cannot be more than 50 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }
    if (isValid) {
        streetAddressInput.classList.remove('error-border');
        streetAddressInput.classList.add('valid-border');
    } else {
        streetAddressInput.classList.remove('valid-border');
        streetAddressInput.classList.add('error-border');
    }

    submitButton.disabled = !isValid;
}

// validation for city 
function validateCity() {
    const cityInput = document.getElementById('city');
    const errorSpan = document.getElementById('cityError');
    const submitButton = document.querySelector('.submit-button');
    const value = cityInput.value;

    // Regular expression to check for valid characters (letters and spaces only)
    const regex = /^[A-Za-z\s]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'City cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'City can only contain letters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'at least 3 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 15) {
        errorSpan.textContent = 'maximum 15 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    if (isValid) {
        cityInput.classList.remove('error-border');
        cityInput.classList.add('valid-border');
    } else {
        cityInput.classList.remove('valid-border');
        cityInput.classList.add('error-border');
    }
    
    submitButton.disabled = !isValid;
}

// validation for state 
function validateState() {
    const stateInput = document.getElementById('state');
    const errorSpan = document.getElementById('stateError');
    const submitButton = document.querySelector('.submit-button');
    const value = stateInput.value;

    // Regular expression to check for valid characters (letters and spaces only)
    const regex = /^[A-Za-z\s]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'State cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'only alphabets.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'least 3 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 15) {
        errorSpan.textContent = 'maximum 15 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    if (isValid) {
        stateInput.classList.remove('error-border');
        stateInput.classList.add('valid-border');
    } else {
        stateInput.classList.remove('valid-border');
        stateInput.classList.add('error-border');
    }
    

    submitButton.disabled = !isValid;
}

// validation for source of discovery 
function validateSourceOfDiscovery() {
    const sourceInput = document.getElementById('sourceOfDiscovery');
    const errorSpan = document.getElementById('sourceOfDiscoveryError');
    const submitButton = document.querySelector('.submit-button');
    const value = sourceInput.value;

    // Regular expression to check for valid characters (letters and spaces only)
    const regex = /^[A-Za-z\s]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Source of Discovery cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Source of Discovery can only contain alphabets.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'Must be at least 3 characters long.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 15) {
        errorSpan.textContent = 'Cannot be more than 15 characters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    if (isValid) {
        sourceInput.classList.remove('error-border');
        sourceInput.classList.add('valid-border');
    } else {
        sourceInput.classList.remove('valid-border');
        sourceInput.classList.add('error-border');
    }
    

    submitButton.disabled = !isValid;
}

// validation for company 
function validateCompany() {
    const companyInput = document.getElementById('company');
    const errorSpan = document.getElementById('companyError');
    const submitButton = document.querySelector('.submit-button');
    const value = companyInput.value;

    // Regular expression to check for valid characters (letters and spaces only)
    const regex = /^[A-Za-z\s]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Company/Organization cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Company/Organization can only contain alphabets.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'Company/Organization must be at least 3 characters long.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 15) {
        errorSpan.textContent = 'Company/Organization cannot be more than 15 characters long.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }
    if (isValid) {
        companyInput.classList.remove('error-border');
        companyInput.classList.add('valid-border');
    } else {
        companyInput.classList.remove('valid-border');
        companyInput.classList.add('error-border');
    }
    

    submitButton.disabled = !isValid;
}

// validation for members 
function validateMembers() {
    const membersInput = document.getElementById('members');
    const errorSpan = document.getElementById('membersError');
    const submitButton = document.querySelector('.submit-button');
    const value = membersInput.value;

    // Regular expression to check for numbers only
    const regex = /^[0-9]+$/;

    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Members cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value !== value.trim()) {
        errorSpan.textContent = 'No leading or trailing spaces allowed.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (!regex.test(value)) {
        errorSpan.textContent = 'Members can only contain numbers.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        const numberValue = parseInt(value, 10);
        if (numberValue < 1) {
            errorSpan.textContent = 'Members must be at least 1.';
            errorSpan.style.display = 'inline';
            isValid = false;
        } else if (numberValue > 100) {
            errorSpan.textContent = 'Members cannot be more than 100.';
            errorSpan.style.display = 'inline';
            isValid = false;
        } else {
            errorSpan.textContent = '';
            errorSpan.style.display = 'none';
        }
    }
    if (isValid) {
        membersInput.classList.remove('error-border');
        membersInput.classList.add('valid-border');
    } else {
        membersInput.classList.remove('valid-border');
        membersInput.classList.add('error-border');
    }
    
    submitButton.disabled = !isValid;
}

// validation for special instructions 
function validateSpecialInstructions() {
    const instructionInput = document.getElementById('special-instruction');
    const errorSpan = document.getElementById('specialInstructionError');
    const submitButton = document.querySelector('.submit-button');
    const value = instructionInput.value.trim();

    // Validation rules
    let isValid = true;

    if (value === '') {
        errorSpan.textContent = 'Special Instructions cannot be empty.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length < 3) {
        errorSpan.textContent = 'Special Instructions must be at least 3 characters long.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (value.length > 50) {
        errorSpan.textContent = 'Special Instructions cannot be more than 50 characters long.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else if (/^[0-9]+$/.test(value)) {
        // Check if the input is fully numeric
        errorSpan.textContent = 'Special Instructions must include letters.';
        errorSpan.style.display = 'inline';
        isValid = false;
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }

    // Add or remove error-border and valid-border classes
    if (isValid) {
        instructionInput.classList.remove('error-border');
        instructionInput.classList.add('valid-border');
    } else {
        instructionInput.classList.remove('valid-border');
        instructionInput.classList.add('error-border');
    }

    submitButton.disabled = !isValid;
}

// checkbox validation 
function validateCheckboxes() {
    const checkboxes = document.querySelectorAll('.hotspot-checkbox');
    const errorSpan = document.getElementById('hotspotError');
    let checked = false;

    // Check if at least one checkbox is checked
    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            checked = true;
        }
    });

    if (!checked) {
        errorSpan.textContent = 'required.';
        errorSpan.style.display = 'inline';
        return false; // Prevent form submission
    } else {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
        return true; // Allow form submission
    }
}

// Add a submit event listener to the form
function validateForm() {
    let isValid = true;

    if (!validateFirstName()) isValid = false;
    if (!validateLastName()) isValid = false;
    if (!validatePhoneNumber()) isValid = false;
    if (!validateEmail()) isValid = false;
    if (!validateStreetAddress()) isValid = false;
    if (!validateCity()) isValid = false;
    if (!validateState()) isValid = false;
    if (!validateSourceOfDiscovery()) isValid = false;
    if (!validateCompany()) isValid = false;
    if (!validateMembers()) isValid = false;
    if (!validateSpecialInstructions()) isValid = false;
    if (!validateCheckboxes()) isValid = false;

    return isValid;
}

// Add a submit event listener to the form
document.querySelector('form').addEventListener('submit', function(event) {
    if (!validateForm()) {
        event.preventDefault(); // Prevent form submission if validation fails
    }
});



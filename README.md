# cms-code-monkeys
CMS project for Team Code Monkeys

Team Behind the Application
This application was built with passion and dedication by a talented team of developers, designers, and contributors. The Code Monkeys CMS team is committed to delivering a secure, user-friendly, and reliable platform for managing user accounts and personal data.

### **Team Members:**
**Ulf Bourelius**

Role:

Full Stack Developer

https://www.linkedin.com/in/ulf-bourelius-a9090248/

---

**Angelia Burgos Zamora**

Role:

Full Stack Developer/ UX & Product Owner 

https://www.linkedin.com/in/angelia-burgos-zamora/

---

**Felix Edenborgh**

Role: 

Full Stack Developer & Scrum Master 

https://www.linkedin.com/in/felix-edenborgh/

---

**Andreas Hohwü-Christensen**

Role: 

Full Stack Developer & Tester 

https://www.linkedin.com/in/andreas-hohwü-christensen-662311240/

---

**Olle Tengnér**

Role: 

Full Stack Developer 

https://www.linkedin.com/in/olle-tengnér-331835175/

---

Below is a comprehensive informational page that combines all the details from the various pages we've built:

---

### **Code Monkeys CMS - User Account Management Overview**

Welcome to the **Code Monkeys CMS User Account Management** section, where you can manage your account settings, security, and personal data. This guide outlines the key features and options available to you through our account management system. Below you will find descriptions of each page and feature related to managing your account in the CMS.

---

### **1. Log In**
**Page:** `/Account/Login`

Log into your Code Monkeys CMS account using your email and password. If your account is protected by two-factor authentication (2FA), you may be required to provide an authentication code or recovery code. For users without an account, a registration link is available.

**Features:**
- Local account login via email and password.
- Support for two-factor authentication (2FA).
- Options to log in with external services.
- Links to reset password and resend confirmation emails.

---

### **2. Register**
**Page:** `/Account/Register`

Create a new Code Monkeys CMS account by providing an email, password, and selecting a user role. An email confirmation link will be sent to verify your account. Optionally, you can also log in using external authentication providers.

**Features:**
- Email verification.
- Custom role selection during registration.
- Ability to log in using external providers like Google or Microsoft.

---

### **3. External Login**
**Page:** `/Account/ExternalLogin`

Authenticate using external providers (e.g., Google or Microsoft). After successfully logging in with an external provider, you'll be prompted to associate the account with an email for the CMS.

**Features:**
- Support for multiple external authentication providers.
- Ability to link external logins to your CMS account.

---

### **4. Forgot Password**
**Page:** `/Account/ForgotPassword`

If you have forgotten your password, you can request a password reset email by providing your registered email address. The email will contain a link to reset your password.

**Features:**
- Reset password via email.
- Validation to ensure proper email address input.

---

### **5. Reset Password**
**Page:** `/Account/ResetPassword`

Follow the instructions sent to your email to reset your password. You will need to provide the email associated with your account and the reset code from the email.

**Features:**
- Password reset functionality.
- Email confirmation and security token validation.

---

### **6. Two-Factor Authentication (2FA) Management**
**Pages:**
- `/Account/Manage/TwoFactorAuthentication`
- `/Account/LoginWith2fa`
- `/Account/LoginWithRecoveryCode`

Two-factor authentication enhances your account security. Set up 2FA using an authenticator app, manage recovery codes, and disable or reset 2FA if necessary.

**Features:**
- Enable and disable 2FA.
- Generate and manage recovery codes.
- Reset 2FA for your account.

---

### **7. Profile and Personal Data**
**Pages:**
- `/Account/Manage`
- `/Account/Manage/PersonalData`
- `/Account/Manage/DeletePersonalData`

Manage your profile, update your phone number, and download or delete your personal data. Deleting personal data will permanently remove your account from the system.

**Features:**
- View and edit phone number and personal data.
- Download or delete personal data.
- Permanent account deletion.

---

### **8. Email Management**
**Page:** `/Account/Manage/Email`

Update your email address and verify your new email address by receiving a confirmation link. If your current email is unverified, you will have the option to resend the verification email.

**Features:**
- Change and verify your email address.
- Receive email confirmation links.

---

### **9. Password Management**
**Pages:**
- `/Account/Manage/ChangePassword`
- `/Account/Manage/SetPassword`

Change your current password or set a new one if you do not have a password associated with your account (e.g., if you registered using an external login provider).

**Features:**
- Change or set a new password.
- Old password verification required to change password.

---

### **10. Authenticator Setup**
**Page:** `/Account/Manage/EnableAuthenticator`

Set up an authenticator app for two-factor authentication. Follow the steps to scan a QR code or input a shared key to configure your 2FA setup.

**Features:**
- Configure your account with an authenticator app.
- Generate recovery codes for 2FA.
- QR code and key-based setup.

---

### **11. Disable 2FA**
**Page:** `/Account/Manage/Disable2fa`

Disable two-factor authentication on your account. Disabling 2FA does not reset the keys used in authenticator apps. If you wish to reset the keys, you will need to reset your authenticator configuration.

**Features:**
- Disable two-factor authentication.
- Information about resetting authenticator keys.

---

### **12. Delete Personal Data**
**Page:** `/Account/Manage/DeletePersonalData`

Permanently delete your personal data and close your account. This action cannot be undone, and you will lose access to your account.

**Features:**
- Permanent account deletion.
- Password confirmation required.

---

### **13. Email Confirmation**
**Pages:**
- `/Account/ConfirmEmail`
- `/Account/ConfirmEmailChange`

Confirm your email address or email change by following the link sent to your email. If successful, your email will be verified, and you will be granted full access to your account.

**Features:**
- Confirm email or email change.
- Link verification and security token validation.

---

### **14. Account Lockout**
**Page:** `/Account/Lockout`

If you attempt to log in with incorrect credentials too many times, your account may be locked. This page notifies you that your account is locked and provides guidance on when you can try again.

**Features:**
- Account lockout notification.
- Information on retrying after a lockout period.

---

### **15. Access Denied**
**Page:** `/Account/AccessDenied`

If you attempt to access a resource for which you do not have the appropriate permissions, you will see this message.

**Features:**
- Access denied notification.
- Information on resource restrictions.

---

### **16. Invalid Password Reset**
**Page:** `/Account/InvalidPasswordReset`

If you try to reset your password with an invalid or expired link, you will be redirected to this page.

**Features:**
- Notification about invalid password reset links.

---

### **17. Invalid User**
**Page:** `/Account/InvalidUser`

This page appears when the system cannot find the user associated with the provided credentials or actions.

**Features:**
- Invalid user notification.
- Prompt to verify user information.

---

### **18. Post-Confirmation**
**Page:** `/PostConfirmation`

After successfully confirming your email or other account actions, you will be redirected to this page while the system checks and finalizes the redirection based on your roles (e.g., admin or user).

---

### **19. Recovery Code Verification**
**Page:** `/Account/LoginWithRecoveryCode`

Log in using one of your recovery codes if you have lost access to your two-factor authentication device.

**Features:**
- Recovery code-based login.
- Security precautions for logging in with recovery codes.

---

### **20. External Logins**
**Page:** `/Account/Manage/ExternalLogins`

Manage your external logins (e.g., Google, Microsoft). You can add or remove external login providers linked to your account.

**Features:**
- View and manage external logins.
- Link or remove external login providers.

---

### Conclusion:
This guide summarizes the key account management features available in Code Monkeys CMS. With various options for security, data management, and profile customization, our system ensures your account remains safe and easy to manage. For additional support, please contact our helpdesk.

--- 

This page provides a detailed description of all account-related management and security features, helping users navigate the Code Monkeys CMS platform.


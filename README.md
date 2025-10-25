![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/dante-signal31/rpmsign)
[![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](https://opensource.org/licenses/BSD-3-Clause)
[![GitHub issues](https://img.shields.io/github/issues/dante-signal31/rpmsign)](https://github.com/dante-signal31/rpmsign/issues)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/y/dante-signal31/rpmsign)](https://github.com/dante-signal31/rpmsign/commits/main)
[![GitHub last commit](https://img.shields.io/github/last-commit/dante-signal31/rpmsign)](https://github.com/dante-signal31/rpmsign/commits/main)

# rpmsign

A GitHub Action to sign RPM packages using passphrase protected GPG keys.

By default *rpm* command ask you to enter a passphrase through console when
you use it to sign a package with a protected GPG key. This behaviour cannot 
be changed and that is a huge problem when you want to sign a package in CI/CD
context, because in an automated workflow you won't be there to enter the 
passphrase.

This action runs the rpm signing command and enters passphrase for 
you when rpm ask for it. This way you can include rpm signing in your
automated workflows.

## Inputs

**Required:**
* *gpg_private_key*: GPG private key file to be used to sign, in armor protected format.
* *gpg_private_key_password*: Passphrase to use to sign with this private key.
* *gpg_name*: Name to use to sign.

**Optional:**
* *rpm_file*: Rpm file to be signed.
* *rpm_folder*: Folder with rpm files to be signed.
* *output_folder*: Folder where signed rpm files should be placed. Defaults to "signed_packages".

## Usage
To sign just a package do:

```yaml
 - name: Sign all RPM packages at Packages folder.
   uses: dante-signal31/rpmsign@v1.0.3
   with:
     gpg_private_key: ${{ secrets.PRIVATE_GPG_KEY }}
     gpg_private_key_password: ${{ secrets.GPG_KEY_PASSWORD }}
     gpg_name: Dante Signal31 <dante.signal31@gmail.com>
     rpm_file: src/tests/resources/packages/package_without_sign/esl-erlang-compat-21.2.6-1.noarch.rpm
     output_folder: SignedPackages/
```
You'll find your signed package at SignedPackages folder. Original unsigned file is kept at its former location.

To sign every package found at, for example Packages/ folder, do:
```yaml
 - name: Sign all RPM packages at Packages folder.
   uses: dante-signal31/rpmsign@v1.0.3
   with:
     gpg_private_key: ${{ secrets.PRIVATE_GPG_KEY }}
     gpg_private_key_password: ${{ secrets.GPG_KEY_PASSWORD }}
     gpg_name: Dante Signal31 <dante.signal31@gmail.com>
     rpm_folder: Packages/
     output_folder: SignedPackages/
```
With that configuration, every RPM package found at Packages/ folder is going to be
copied to SignedPackages folder and signed there.

Be aware that you should enter *gpg_private_key_password* 
through a secret. For *gpg_private_key* I usually create a file on the fly from a secret and I remove it after rpmsign 
has used it. Never hard code those parameters in your source, because they would be exposed to whoever reads 
your source.

The *gpg_private_key* file should be in protected ASCII armor format. You know,
the one with this format:
``` 
-----BEGIN PGP PRIVATE KEY BLOCK-----

[...]
-----END PGP PRIVATE KEY BLOCK-----
```

**CAUTION:** GitHub mangles armor formatted private keys when stored in secrets. So if you echo that secret in a file
and pass that file to rpmsign, you will get an error. The trick is to store the armor private key in a secret encoded 
in base 64. Just do in your console:

```commandline
base64 -i test_priv.gpg -o test_priv.gpg.base64
```

You can store the content of base64 encoded file (in our example test_priv.gpg.base64) in your secret . To recover the 
private key file from your secret, echo the secret variable through base64 (in decoding mode) before passing it to 
rpmsign:

```commandline
echo $gpg_private_key | base64 -di > recovered_priv_key.gpg
```

---
If you want to use rpmsign for other CI/CD platforms instead GitHub Actions, you can use directly
its docker image. You can find it at: https://hub.docker.com/repository/docker/dantesignal31/rpmsign/general

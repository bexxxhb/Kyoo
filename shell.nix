{pkgs ? import <nixpkgs> {}}: let
  venvDir = "./.venv";
  pythonPkgs = ./scanner/requirements.txt;
in
  pkgs.mkShell {
    packages = with pkgs; [
      nodejs-16_x
      nodePackages.yarn
      (with dotnetCorePackages;
        combinePackages [
          sdk_6_0
          aspnetcore_6_0
        ])
      python3
    ];

    shellHook = ''
      # Install python modules
      SOURCE_DATE_EPOCH=$(date +%s)
      if [ ! -d "${venvDir}" ]; then
        ${pkgs.python3} -m venv "${venvDir}"
      fi
      source "${venvDir}/bin/activate"
	  export PIP_DISABLE_PIP_VERSION_CHECK=1
      pip install -r ${pythonPkgs}
    '';
  }

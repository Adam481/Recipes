# VS Code initial configuration


### Recommended packages
```ps
code --install-extension donjayamanne.jquerysnippets && 
code --install-extension DotJoshJohnson.xml && 
code --install-extension ecmel.vscode-html-css && 
code --install-extension eg2.vscode-npm-script && 
code --install-extension HookyQR.beautify && 
code --install-extension JLattimer.dynamics-crm-js && 
code --install-extension lonefy.vscode-JS-CSS-HTML-formatter && 
code --install-extension ms-mssql.mssql && 
code --install-extension ms-vscode.csharp && 
code --install-extension ms-vscode.PowerShell && 
code --install-extension msjsdiag.debugger-for-chrome && 
code --install-extension robertohuertasm.vscode-icons && 
code --install-extension thekalinga.bootstrap4-vscode && 
code --install-extension thomaspink.theme-github && 
code --install-extension wcwhitehead.bootstrap-3-snippets && 
code --install-extension xabikos.JavaScriptSnippets && 
code --install-extensionyzhang.markdown-all-in-one
```

### Constom configuration example

```json
{
    "editor.fontSize": 14,
    "editor.lineHeight": 0,
    "editor.wordWrapColumn": 100,
    "editor.trimAutoWhitespace": true,
    "editor.quickSuggestions": true,
    "editor.acceptSuggestionOnEnter": "on",
    "editor.tokenColorCustomizations": {
        "comments": "#56595D"
    },
    "editor.mouseWheelZoom": true,
    "editor.renderIndentGuides": true,
    "editor.minimap.maxColumn": 50,
    "editor.minimap.showSlider": "always",
    "editor.glyphMargin": false,

    "editor.insertSpaces": true,
    "editor.renderWhitespace": "boundary",
    "editor.tabSize": 2,
    "editor.detectIndentation": true,
    "workbench.iconTheme": "vscode-icons",
    "workbench.editor.enablePreview": false,
    "window.zoomLevel": -1,
    "window.menuBarVisibility": "toggle",
    "files.autoSave": "onFocusChange",
    "files.eol": "\r\n",

    "mssql.connections": [
        {
            "server": "xxx",
            "database": "xxx",
            "authenticationType": "xxx",
            "profileName": "xxx",
            "password": ""
        }
    ],
    "workbench.panel.location": "bottom",

    "eslint.enable": true,
    "eslint.autoFixOnSave": true,
    "eslint.options": {}
}
```

### Installing airbnb linting style


#### Installing required packages
```ps
npm install -g eslint-config-airbnb-es5 babel-eslint eslint-plugin-react
```

#### Configuring user global configuration for linting

```json
{
    "extends": "eslint-config-airbnb-es5",
    ....
    "rules": {
        "eqeqeq": 2,
        "comma-dangle": 1,
        "no-console": 0,
        "no-debugger": 1,
        "no-extra-semi": 1,
        "no-extra-parens": 1,
        "no-irregular-whitespace": 0,
        "no-undef": 0,
        "no-unused-vars": 0,
        "semi": 1,
        "semi-spacing": 1,
        "valid-jsdoc": [
            2,
            { "requireReturn": false }
        ],

        "react/display-name": 2,
        "react/forbid-prop-types": 1,
        "react/jsx-boolean-value": 1,
        "react/jsx-closing-bracket-location": 1,
        "react/jsx-curly-spacing": 1,
        "react/jsx-indent-props": 1,
        "react/jsx-max-props-per-line": 0,
        "react/jsx-no-duplicate-props": 1,
        "react/jsx-no-literals": 0,
        "react/jsx-no-undef": 1,
        "react/sort-prop-types": 1,
        "react/jsx-sort-props": 0,
        "react/jsx-uses-react": 1,
        "react/jsx-uses-vars": 1,
        "react/no-danger": 1,
        "react/no-did-mount-set-state": 1,
        "react/no-did-update-set-state": 1,
        "react/no-direct-mutation-state": 1,
        "react/no-multi-comp": 1,
        "react/no-set-state": 0,
        "react/no-unknown-property": 1,
        "react/prop-types":0,
        "react/react-in-jsx-scope": 0,
        "react/require-extension": 0,
        "react/self-closing-comp": 1,
        "react/sort-comp": 1,
        "react/jsx-wrap-multilines": 1
    }
}
```

import { Scene } from 'phaser'
import InputText from 'phaser3-rex-plugins/plugins/inputtext.js';
import { Buttons, Label } from 'phaser3-rex-plugins/templates/ui/ui-components.js';

import { StartSocket, Authenticate } from '../utils/connection'

import font from '../assets/font/font1.png';
import fontData from '../assets/font/font1.xml';

export default class MenuScene extends Scene {
  constructor() {
    super({ key: 'MenuScene' })
  }

  preload() {
    this.load.bitmapFont('font', font, fontData);
  }

  create() {
    const center = this.game.canvas.width / 2

    this.add.bitmapText(center, 76, 'font', "Margarina Quest", 48).setOrigin(0.5, 0.5)

    this.usernameInput = new InputText(this, center, 180, 140, 26, { placeholder: "Username", color: '#ffffff', backgroundColor: 'grey', fontFamily: 'm3x6', fontSize: "32px", align: "center" })
    this.passwordInput = new InputText(this, center, 210, 140, 26, { placeholder: "password", color: '#ffffff', backgroundColor: 'grey', fontFamily: 'm3x6', fontSize: "32px", align: "center" })

    this.usernameInput.node.maxLength = 16;
    this.usernameInput.node.autocomplete = "off"

    this.passwordInput.node.type = "password"
    this.passwordInput.node.autocomplete = "off"

    this.add.existing(this.usernameInput);
    this.add.existing(this.passwordInput);

    let buttonShape = this.add.rectangle(0, 0, 110, 1, 0x7b5e57)

    var loginButton = new Label(this, {
      width: 140,
      height: 26,
      align: 'center',
      background: buttonShape,
      text: this.add.bitmapText(0, 0, 'font', "Login", 16)
    });

    var buttons = new Buttons(this, {
      x: center,
      y: 240,
      buttons: [
        loginButton
      ],
      align: center,
      expandTextHeight: false,
    }).layout()

    buttons.on('button.click', () => this.login())

    this.add.existing(buttons);

    this.usernameInput.node.focus()
    this.passwordInput.node.onkeypress = event => {
      if (event.code === "Enter") {
        this.login()
      }
    }
  }

  update() { }

  login() {
    this.scene.stop('MenuScene')
    this.scene.start('LoadingScene')

    var username = this.usernameInput.text
    var passsword = this.passwordInput.text

    Authenticate(username, passsword)
      .then(response => StartSocket(response.token))
      .then(connection => {
        this.scene.start('GameScene', { connection: connection, username: username })
        this.scene.stop('LoadingScene')
      })
      .catch(error => {
        this.scene.start('MenuScene')
        this.scene.stop('LoadingScene')
        console.error(error)
      })
  }

}
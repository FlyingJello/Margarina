import { Scene } from 'phaser'
import InputText from 'phaser3-rex-plugins/plugins/inputtext.js';

import mapTiles from '../assets/sprites/terrain/tiles_sewers-extruded.png';
import level from '../assets/levels/test_level.json';
import ranger from '../assets/sprites/ranger.png';
import font from '../assets/font/font1.png';
import fontData from '../assets/font/font1.xml';

import Actor from '../models/actor';
import Player from '../models/player';

export default class GameScene extends Scene {
  constructor() {
    super({
      type: Phaser.AUTO,
      key: 'GameScene',
      physics: {
        default: 'arcade',
        arcade: { gravity: { y: 0 } }
      }
    })

    this.actors = []
  }

  init({ connection, username }) {
    this.connection = connection
    this.playerId = username

    connection.onclose(() => {
      scene.scene.stop('GameScene')
      scene.scene.start('MenuScene')
    });
  }

  preload() {
    this.load.image('tiles', mapTiles);
    this.load.tilemapTiledJSON('map', level);
    this.load.spritesheet('player', ranger, { frameWidth: 16, frameHeight: 16 });
    this.load.bitmapFont('font', font, fontData);
  }

  create() {
    this.map = this.make.tilemap({ key: 'map' });

    const tileset = this.map.addTilesetImage('sewers', 'tiles', 16, 16, 1, 2);
    const layers = this.map.layers.filter(layer => layer.visible).map(layer => this.map.createStaticLayer(layer.name, tileset, 0, 0))

    layers.find(tileLayer => tileLayer.layer.name === "top").setDepth(69)

    let player = new Player(this, this.playerId, { x: 0, y: 0 })
    this.actors.push(player)

    this.cameras.main.setBounds(0, 0, this.map.widthInPixels, this.map.heightInPixels);
    this.cameras.main.startFollow(player.group, true);

    //player anims
    this.anims.create({
      key: 'walk',
      frames: this.anims.generateFrameNumbers('player', { start: 2, end: 7 }),
      frameRate: 10,
      repeat: -1
    });

    this.anims.create({
      key: 'idle',
      frames: [{ key: 'player', frame: '0' }],
      frameRate: 10,
    });

    //debug graphics
    this.selector = this.add.graphics({ lineStyle: { width: 1, color: 0xffffff, alpha: 1 } });
    this.selector.strokeRect(0, 0, this.map.tileWidth, this.map.tileHeight);

    this.destRect = this.add.graphics({ lineStyle: { width: 1, color: 0xffffff, alpha: 1 } });
    this.destRect.strokeRect(0, 0, this.map.tileWidth, this.map.tileHeight);

    //pointer handling
    this.input.on('pointerdown', function (pointer) {
      let pointerTile = { x: this.map.worldToTileX(pointer.x + this.cameras.main.worldView.x), y: this.map.worldToTileY(pointer.y + this.cameras.main.worldView.y) }
      this.connection.invoke("move_request", pointerTile)
    }, this);

    //game tick handling
    this.connection.on("tick", state => {
      //update actors
      let actorsToRemove = this.actors.filter(actor => !state.actors.find(act => act.id === actor.id))
      this.actors = this.actors.filter(actor => !actorsToRemove.find(act => act.id === actor.id))
      actorsToRemove.forEach(actor => actor.destroy())

      let newActors = state.actors.filter(actor => !this.actors.find(act => act.id === actor.id)).map(actor => new Actor(this, actor.id, 'player', actor.position))
      this.actors = this.actors.concat(newActors)

      this.actors.forEach(actor => actor.update(state.actors.find(act => act.id === actor.id)))

      //chat
      let player = state.actors.find(actor => actor.id === this.playerId)
      this.chat.text = player.chatHistory.slice(Math.max(player.chatHistory.length - 8, 0))
    });

    //UI section
    //chat
    let chatBox = this.add.container(0, this.game.canvas.height - 120)
    chatBox.setScrollFactor(0, 0);

    let chatInput = new InputText(this, 0, 104, 240, 16, { color: '#ffffff', backgroundColor: 'grey', fontFamily: 'm3x6l', fontSize: "16px" }).setOrigin(0, 0)
    chatInput.node.autocomplete = "off"
    this.add.existing(chatInput);
    chatBox.add(chatInput)

    chatInput.node.onkeypress = event => {
      if (event.code === "Enter") {
        this.connection.invoke("chat", chatInput.node.value)
        chatInput.node.value = ""
      }
    }

    this.chat = this.add.bitmapText(0, 0, 'font', [], 8)
    chatBox.add(this.chat)
  }

  update() {
    //draw the tile selector
    let worldPoint = this.input.activePointer.positionToCamera(this.cameras.main);
    let pointerTile = { x: this.map.worldToTileX(worldPoint.x), y: this.map.worldToTileY(worldPoint.y) }
    this.selector.x = this.map.tileToWorldX(pointerTile.x);
    this.selector.y = this.map.tileToWorldY(pointerTile.y);

    // draw destination DEBUG
    // let player = this.actors.find(actor => actor.id === this.playerId)
    // this.destRect.setPosition(this.map.tileToWorldX(player.currentAction?.nextStep.x), this.map.tileToWorldY(player.currentAction?.nextStep.y))
  }
}
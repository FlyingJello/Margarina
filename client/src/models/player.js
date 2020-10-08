import Actor from "./actor";

export default class Player extends Actor {
  constructor(game, name, position) {
    super(game, name, 'player', position)

    this.name = name

    let nametag = game.add.bitmapText(8, -10, 'font', name, 8).setOrigin(0.5, 0.5);
    nametag.setDepth(99)
    this.group.add(nametag);
  }
}
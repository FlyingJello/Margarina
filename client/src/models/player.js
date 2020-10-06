import Actor from "./actor";

export default class Player extends Actor {
  constructor(game, name, position) {
    super(game, name, 'player', position)

    this.name = name

    let nametag = game.add.text(8, -10, name, { fontFamily: 'm3x6l' }).setOrigin(0.5, 0.5);
    nametag.setDepth(99)
    this.group.add(nametag);
  }
}
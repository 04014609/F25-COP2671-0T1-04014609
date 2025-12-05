
Project Overview
==============
This project is a small 2D farming simulation built in Unity.
The player can move around, interact with farming tiles, plant seeds, water crops, harvest them when mature, and collect items into an inventory system.
A time cycle controls crop growth and day/night events.

# How to Play the Game
Movement Controls

Up Arrow – Move up

Left Arrow – Move left

Down Arrow – Move down

Right Arrow – Move right

Farming Tools (Toolbar)

Click the on-screen buttons to select a tool:

Hoe – Plows an empty tile

Water – Waters plowed/planted soil

Plant – Plants the currently selected seed type

Harvest – Harvests mature crops

The selected tool darkens to show it is active.

# Inventory

Press W to toggle the inventory panel (when not moving) - Might need to set inventory panel to active in the cavas for it to work

Harvested items appear with:

Icon

Name

Quantity

Inventory updates automatically whenever new items are collected.

# Crop Growth System

Crops progress through multiple growth stages:

Seed

Sprout

Young

Mature

Growth updates once per in-game day, triggered by the time system.
Mature crops display a particle effect and can be harvested.

# Time System

The TimeManager simulates an in-game clock:

1 real second = 1 in-game minute (adjustable)

At 24 in-game hours, a new day begins

OnDayPassed event triggers crop growth

A DayNightEvents script triggers sunrise and sunset events, which can be used for lighting changes.

# Inventory System (Technical Summary)

Uses a list of InventorySlot objects

Adds new items or increases quantity if already collected

Fires an event OnInventoryChanged

Inventory UI rebuilds itself each time the event fires

# ScriptableObjects Used

Two types of ScriptableObjects support easy content creation:

SeedPacket – defines crop name, growth sprites, growth stages, harvest prefab

HarvestItem – defines item name, icon, and value

To add new crops, create new SeedPacket and HarvestItem assets.

# My small additions
* Added an audio component for a bakcground music that plays in the backgrownd
* A pop up screen that states "Good morning" after the sun goes down at 00:00 
* A small particle effect when crops are ready to harvest
* My own sprites for the inventory panel + inventory slots

# External Function Declarations
EXTERNAL SET_FLAG(chapterId, missionId, flagName)
EXTERNAL GET_FLAG(chapterId, missionId, flagName)

===0_0_0_Intro===

#blackout on

#speakers Player Professor

Player: Professor, for this Survey in Bayvan… is it just a few sample points and a report?
Player: Sounds like my thesis is going to be easier than I thought.

The Professor doesn’t turn around right away, pausing mid-motion.
Professor: …Samples, yes. Records, too.

He turns to face you, holding a slightly worn, intricately structured device.
Professor: Use this. The core recording unit.
Player: The Sphere? Isn’t that… the old ‘Living Recorder’ project that got shelved? I heard there were… incidents.
Professor: It records resonance. Truer than data…Especially after disasters.

Research my thesis using that blacklisted device? Well… He knew the accident briefings and never brought them up.
Bayvan… Is there something significant there for you?

-> DONE

===0_1_0_VC_Outside===

Bayvan Visitor Center exterior. 
The sunlight is glaring, the waves dull, along with some slight undercurrent in the air.

#speakers Player

Player: Huh, the visitor center’s newer than I expected. Not bad.
Player: Though that murky sea… the dead pier… and that smell in the air…
Player: Seems Professor really brought something for me.
Player: Guess I'd better go find Ms. Ella. He said she’s handling my onboarding. 
Player: Let's see if there's anyone in the visitor center that knows where she is.

-> DONE

===0_1_1_VC_Broadcast===

A sweet voice from a speaker:
Welcome to Bayvan Eco-Park! Experience pristine nature and enjoy the harmony of coexistence! Today’s feature: “The Mayor and the Sea” photography exhibit—relive our pioneer glory!

#speakers Player
Player: Pristine? Coexistence? Really?… Eco-orb, you’re the “Living Recorder.” Got anything to say?
-> DONE


===0_1_2_FrontDesk===

# speakers Player FD

FD: ...Yes, yes, Ella! The person is here—looks like a student... holding a weird ball? ...Got it, I'll say you were delayed by an urgent interview at the sewage plant, and tell him to—what? Take photos of the sediment? Noted, noted!

She sees you, immediately hangs up, takes a deep breath, and forces a professional smile.

FD: Sorry for the wait! Miss Ella was just called away for an urgent interview at the sewage plant! She apologizes and said you can go meet her directly in the new district!

She hands you a city map and a business card.

FD: It's at this address, really easy to find!

//#show_popup "Received Map"

//#Enable Map

#show_popup "Received Ella name card"

#add_notebook Ella_name_card

You take the note. Suddenly, your phone vibrates violently in your pocket.

Player: ...?

Player: Urgent interview at the sewage plant? Really...?

Your phone screen lights up—a new email. Sender: [Data Deleted], Subject: For the person with the ball.

#show_popup "Received UnknownMail"

#add_notebook UnknownMail

#show_item UnknownMail

An Unknown Mail 

Email Body: Research station, Basement Level 3, Section B, sample port of the recirculating water tank. This is a real water sample. Don’t trust their "qualified" report. The arsenic adsorption data was manually smoothed. The attachment is the original graph.  
Email Body: (Attachment: An arsenic concentration monitoring line chart. Most of the line is a smooth green, but one point is circled in red, showing a sudden spike. Beside it is a small note: Sample Point: Yucun Majiajing – 7/14.)

Player: (pupils contract, finger sliding across that glaring red spike on the screen):  
Player: Manually smoothed...? Qiu? The genius sampler Professor mentioned—the one who left the project? He sent me an email? Arsenic... Yucun Majiajing?

~ SET_FLAG(0,1,"phone_call")

-> END

===0_1_3_VC_MateoConflict===

// Triggers immediately upon exiting the visitor center. Background zooms out.
At the dock, a disheveled, middle-aged man hurls a bucket of rotting fish onto the deck of a shiny new tourist yacht!
The fish gills shimmer with strange metallic blue blotches. Several Bayvan tourism staff are around him.

#speakers Mateo Security

Security A: Mateo! What the hell?! The tourists are watching!
Mateo: Let them watch! The blue on those gills? Same crap from your discharge pipes!
Mateo: Three years! What did you bring to us?! Huh?!

Security B: Everything was fine! The experts said it’s a rare red tide—
Mateo: Red tide?! The fish Noah pulled out before he died looked just like this! You gonna say he drowned in a “rare red tide” too?!

The moment he says “Noah,” their faces twist. Movements freeze.
In the chaos, Mateo’s bloodshot eyes lock on the Sphere in your hands.

Mateo: Another damn recorder?! Fine! Great! Tell those lab coats this——Their “clean” data can’t keep a single fish alive.
Mateo: And sure as hell can’t bring Noah back!

The beach  falls silent.
You alone notice the orb trembling slightly in your hands, cloudy swirls rising inside it.

#enable UI Orb
#show_popup "EcoSphere is now active"

-> DONE

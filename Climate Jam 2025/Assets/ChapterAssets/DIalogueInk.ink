# External Function Declarations
EXTERNAL SET_FLAG(chapterId, missionId, flagName)
EXTERNAL GET_FLAG(chapterId, missionId, flagName)

===0_0_0_Intro===

#blackout on

#speakers Player Professor

Player: Professor, for this Survey in Bayvan… is it just a few sample points and a report?
Player: Sounds like my thesis will be easier than I thought.

The Professor doesn’t turn around right away, pausing mid-motion.
Professor: …Samples, yes. But records, too.

He turns to face you, holding a slightly worn, intricately structured device.
Professor: Use this. The core recording unit.

Player: The Sphere? Isn’t that… the old ‘Living Recorder’ project that got shelved? I heard there were… incidents.
Professor: It records resonance. Truth deeper than numbers… Especially after disasters.

Research my thesis using that blacklisted device? Well… He knew about those accident briefings and never said a word.

Bayvan… Is there something there for you, Professor?

-> DONE

===0_1_0_VC_Outside===

Bayvan Visitor Center exterior. 
Sunlight blazes down, waves dull and listless, with something stirring in the air.

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
Player: Pristine? Coexistence? Really?… Hey Sphere, you’re the “Living Recorder.” Got anything to say?
-> DONE


===0_1_2_FrontDesk===

# speakers Player FD

Front Desk: ...Yes, yes, Ella! The person is here—looks like a student... holding a weird ball? ...Got it, I'll say you were delayed by an urgent interview at the sewage plant, and tell him to—what? Take photos of the sediment? Noted, noted!

She sees you, immediately hangs up, takes a deep breath, and forces a professional smile.

Front Desk: Sorry for the wait! Miss Ella was just called away for an urgent interview at the sewage plant! She apologizes and said you can go meet her directly in the new district!

She hands you a business card.

Front Desk: Here's the address, it's really easy to find!

//#show_popup "Received Map"

//#Enable Map

#show_popup "Received Ella name card"

#add_notebook Ella_name_card

You take the note. Suddenly, your phone vibrates violently in your pocket.

Player: ...?

Player: Urgent interview at the sewage plant? Seriously...?

Your phone screen lights up—a new email. 

#show_popup "Received UnknownMail"

#add_notebook UnknownMail

#show_item UnknownMail

Sender: [Data Deleted], Subject: For the person with the sphere. 

Email Body: Research station, Basement Level 3, Section B, sample port of the recirculating water tank. This is a real water sample. Don’t trust their "qualified" report. The arsenic adsorption data was manually smoothed. The attachment is the original graph.  
Email Body: (Attachment: An arsenic concentration monitoring line chart. Most of the line is a smooth green, but one point is circled in red, showing a sudden spike. Beside it is a small note: Sample Point: Yucun Majiajing – 7/14.)

Player: (pupils contract, finger sliding across that glaring red spike on the screen):  
Player: Manually smoothed...? Qiu? The genius sampler Professor mentioned—the one who left the project? He sent me an email? Arsenic... Fisherman’s Village Well?

~ SET_FLAG(0,1,"phone_call")

-> END

===0_1_3_VC_Mateo1===

// Triggers immediately upon exiting the visitor center. Background zooms out.
At the dock, a disheveled, middle-aged man hurls a bucket of rotting fish onto the deck of a shiny new tourist yacht.
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

The beach fell silent.
The orb trembling slightly in your hands, cloudy swirls rising inside it.

#enable sphere
#show_popup "EcoSphere is now active"

-> DONE

===0_2_0_Q2===

#speakers Player Qiu 

Outside the Baylan Laboratory of Environments.
You walk across a gravel path and spot a man standing at the gate.  
Player: Excuse me… Are you Mr Qiu? My Professor said a senior researcher was stationed here.  
Player: He also asked me to bring this along—said maybe it could still be of use.
Qiu: Hmmm…Welcome. 
The moment he sees the Orb on your hand, something shifts subtly in his expression.
Qiu: Well, so he still remembers it.  
Qiu: It’s not that it’s useless. It’s that no one wants to be responsible for what happens after it works.
Player: You mean the incident? The files I found... they’re all redacted.

Qiu: The things you should find, you won’t. And the things you won’t find, I won’t tell.  
Qiu: Let me see... if I remember correctly, it works like this...

You hand him the eco-orb. You notice the suspended particles inside begin to shift. 
Under some strange force, they disassemble and recombine—gradually forming a faint, symbolic pattern.

Qiu: It activates near a live sample source. Just keep it away from strong electromagnetic fields. And whatever you do, don’t let it touch seawater.

Player: My advisor said it records "resonance," not just numbers.

Qiu: Resonance is data. Just not the kind that looks good on a spreadsheet.  
Qiu: But sometimes... what it shows—well, you’ll see once you start recording.

He hands the orb back and gestures toward the beach and the tourist center in the distance.  
Qiu: I doubt you're here just to admire the station. Others have noticed the problem before you did.

Player: Who?

Qiu: A journalist. Name's Ella. She doesn’t wait for permission to start digging.

Player: You know her?

Qiu: She asks the kind of questions you’re carrying too. Maybe the two of you will find some answers together.

He unlocks the door to the research station, but doesn’t step inside right away.  
Qiu: While you’re in Bihaven, I’ll help where I can... Just don’t expect too much.  
Qiu: What you learn depends on what you’re willing to ask. But don’t take too long. Some people are running out of time.

#show_popup("Qiu joined team")  
#addtoparty Qiu

-> DONE


var releasesUrl="https://api.github.com/repos/hypergamesdev/sss222/releases";
var releasesData;
var releasesDataJSON;
var stableUrl="https://api.github.com/repos/hypergamesdev/sss222/releases/latest";
var stableUrlGoto="https://github.com/hypergamesdev/sss222/releases/latest";
var stableData;
var stableTag;
var stableName;
var latestUrlGoto="https://github.com/HyperGamesDev/sss222/releases/tag/";
var latestData;
var latestTag;
var latestName;
var launcherUrl="https://github.com/HyperGamesDev/sss222/releases/download/v0.3-beta/SSS222-Launcher-11.08.rar";
async function getapiReleases(){
	const responseReleases=await fetch(releasesUrl);
	releasesData=await responseReleases.json();
	
	const responseStable=await fetch(stableUrl);
	stableData=await responseStable.json();
	
	console.log("Releases Data");
	console.log(releasesData);
	latestData=releasesData[0];
	console.log("Latest Data");
	console.log(latestData);
	latestTag=latestData.tag_name;
	latestName=(latestData.name).replace("SSS222","");
	
	console.log("Stable Data");
	console.log(stableData);
	stableName=(stableData.name).replace("SSS222","");
	
	//sleep(100);
	setReleasesText();
	setReleasesHref();
}
async function getapiStable(){
	const responseStable=await fetch(stableUrl);
	stableData=await responseStable.json();
	console.log("Stable Data");
	console.log(stableData);
	stableName=(stableData.name).replace("SSS222","");
	
}
getapiReleases();

function setReleasesText(){
	document.getElementById("stableText").innerHTML=stableName;
	document.getElementById("latestText").innerHTML=latestName;
}
function setReleasesHref(){
	document.getElementById("launcher").href=launcherUrl;
	document.getElementById("stable").href=stableUrlGoto;
	latestUrlGoto=latestUrlGoto.concat(latestTag);
	document.getElementById("latest").href=latestUrlGoto;
}

function downloadLauncher(){
	location.href=launcherUrl;
}
function goToStableRelease(){
	location.href=stableUrlGoto;
}
function goToLatestRelease(){
	latestUrlGoto=latestUrlGoto.concat(latestTag);
	location.href=latestUrlGoto;
}

function sleep(milliseconds) {
  const date = Date.now();
  let currentDate = null;
  do {
    currentDate = Date.now();
  } while (currentDate - date < milliseconds);
}
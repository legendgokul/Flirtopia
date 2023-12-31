﻿namespace ApiProject.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; }  
    public string Email { get; set; }
    public byte[] PasswordHash {get;set;}
    public byte[] PasswordSalt {get;set;}
    public DateTime DateOfBirth {get;set;}
    public string knownAs {get;set;}
    public DateTime CreatedOn {get;set;} = DateTime.UtcNow;
    public DateTime LastActive {get;set;} = DateTime.UtcNow;
    public string Gender {get;set;}
    public string Introduction {get;set;}
    public string lookingFor {get;set;}
    public string Interests {get;set;}
    public string City {get;set;}
    public string Country {get;set;}
    public List<Photo> Photos {get;set;} = new List<Photo>();

    //many to many linking for like feature
    public List<UserLike> LikedByUsers {get;set;}
    public List<UserLike> LikedUsers {get;set;}

    //messages
    public List<Message> MessageSent {get;set;}
    public List<Message> MessageReceived   {get;set;} 
}


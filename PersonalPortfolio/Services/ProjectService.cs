// PersonalPortfolio/Services/ProjectService.cs
using PersonalPortfolio.Models;
using System.Collections.Generic;
using System.Linq;

namespace PersonalPortfolio.Services
{
    public class ProjectService
    {
        private readonly List<CodeSnippet> _codeSnippets;
        private readonly List<Project> _projects;
        private readonly List<Experience> _experiences;
        private readonly List<Skill> _skills;
        private readonly List<BlogPost> _blogPosts;

        public ProjectService()
        {
            _codeSnippets = InitializeCodeSnippets();
            _projects = InitializeProjects();
            _experiences = InitializeExperiences();
            _skills = InitializeSkills();
            _blogPosts = InitializeBlogPosts();
        }

        #region Public Methods
        public List<Project> GetAllProjects()
        {
            return _projects;
        }

        public Project GetProjectById(int id)
        {
            return _projects.FirstOrDefault(p => p.Id == id) ?? new Project();
        }

        public List<Project> GetFeaturedProjects(int count = 3)
        {
            return _projects.Where(p => p.Featured).Take(count).ToList();
        }

        public List<Project> SearchProjects(string searchTerm = "", string category = "")
        {
            var filteredProjects = _projects;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredProjects = filteredProjects
                    .Where(p => p.Title.ToLower().Contains(searchTerm) || 
                               p.Description.ToLower().Contains(searchTerm) || 
                               p.DetailedDescription.ToLower().Contains(searchTerm) ||
                               p.Technologies.Any(t => t.ToLower().Contains(searchTerm)))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                filteredProjects = filteredProjects
                    .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return filteredProjects;
        }

        public List<string> GetProjectCategories()
        {
            return _projects.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
        }

        public List<Experience> GetAllExperiences()
        {
            return _experiences;
        }

        public List<Skill> GetAllSkills()
        {
            return _skills;
        }

        public List<Skill> GetSkillsByCategory(string category)
        {
            return _skills.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                          .OrderByDescending(s => s.ProficiencyLevel)
                          .ToList();
        }

        public List<string> GetSkillCategories()
        {
            return _skills.Select(s => s.Category).Distinct().OrderBy(c => c).ToList();
        }

        public List<CodeSnippet> GetAllCodeSnippets()
        {
            return _codeSnippets;
        }

        public CodeSnippet GetCodeSnippetById(int id)
        {
            return _codeSnippets.FirstOrDefault(s => s.Id == id) ?? new CodeSnippet();
        }

        public List<CodeSnippet> SearchCodeSnippets(string searchTerm = "", string language = "", string tag = "")
        {
            var filteredSnippets = _codeSnippets;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredSnippets = filteredSnippets
                    .Where(s => s.Title.ToLower().Contains(searchTerm) || 
                               s.Description.ToLower().Contains(searchTerm) || 
                               s.Code.ToLower().Contains(searchTerm))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                filteredSnippets = filteredSnippets
                    .Where(s => s.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                filteredSnippets = filteredSnippets
                    .Where(s => s.Tags.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            return filteredSnippets;
        }

        public List<string> GetAllLanguages()
        {
            return _codeSnippets.Select(s => s.Language).Distinct().OrderBy(l => l).ToList();
        }

        public List<string> GetAllTags()
        {
            return _codeSnippets.SelectMany(s => s.Tags).Distinct().OrderBy(t => t).ToList();
        }

        public List<BlogPost> GetAllBlogPosts()
        {
            return _blogPosts;
        }

        public BlogPost GetBlogPostById(int id)
        {
            return _blogPosts.FirstOrDefault(b => b.Id == id) ?? new BlogPost();
        }

        public BlogPost GetBlogPostBySlug(string slug)
        {
            return _blogPosts.FirstOrDefault(b => b.Slug == slug) ?? new BlogPost();
        }

        public List<BlogPost> GetFeaturedBlogPosts(int count = 1)
        {
            return _blogPosts.Where(b => b.IsFeatured).Take(count).ToList();
        }

        public List<BlogPost> SearchBlogPosts(string searchTerm = "", string category = "")
        {
            var filteredPosts = _blogPosts.Where(b => !b.IsDraft).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredPosts = filteredPosts
                    .Where(b => b.Title.ToLower().Contains(searchTerm) || 
                               b.Excerpt.ToLower().Contains(searchTerm) || 
                               b.Content.ToLower().Contains(searchTerm))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                filteredPosts = filteredPosts
                    .Where(b => b.Categories.Any(c => c.Equals(category, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            return filteredPosts;
        }

        public List<string> GetBlogCategories()
        {
            return _blogPosts.SelectMany(b => b.Categories).Distinct().OrderBy(c => c).ToList();
        }
        #endregion

        #region Private Initialization Methods
        private List<CodeSnippet> InitializeCodeSnippets()
        {
            return new List<CodeSnippet>
            {
                new CodeSnippet
                {
                    Id = 1,
                    Title = "n8n Workflow Automation Template",
                    Description = "Template for creating notification systems and scheduled tasks in n8n platform",
                    Language = "javascript",
                    Code = @"// n8n Notification Workflow Configuration
{
  ""nodes"": [
    {
      ""parameters"": {
        ""rule"": {
          ""interval"": [
            {
              ""field"": ""cronExpression"",
              ""expression"": ""0 9 * * 1-5""
            }
          ]
        }
      },
      ""name"": ""Schedule Trigger"",
      ""type"": ""n8n-nodes-base.scheduleTrigger"",
      ""position"": [250, 300]
    },
    {
      ""parameters"": {
        ""method"": ""GET"",
        ""url"": ""={{ $json.apiEndpoint }}/events"",
        ""authentication"": ""predefinedCredentialType"",
        ""sendQuery"": true,
        ""queryParameters"": {
          ""parameters"": [
            {
              ""name"": ""date"",
              ""value"": ""={{ new Date().toISOString().split('T')[0] }}""
            },
            {
              ""name"": ""type"",
              ""value"": ""={{ $json.eventType }}""
            }
          ]
        }
      },
      ""name"": ""Fetch Events"",
      ""type"": ""n8n-nodes-base.httpRequest"",
      ""position"": [450, 300]
    },
    {
      ""parameters"": {
        ""conditions"": {
          ""boolean"": [
            {
              ""value1"": ""={{ $json.events.length > 0 }}"",
              ""value2"": true
            }
          ]
        }
      },
      ""name"": ""Has Events"",
      ""type"": ""n8n-nodes-base.if"",
      ""position"": [650, 300]
    },
    {
      ""parameters"": {
        ""operation"": ""send"",
        ""to"": {
          ""value"": ""={{ $node[""Fetch Events""].json.recipients.join(',') }}"",
        },
        ""subject"": ""={{ $json.notificationTitle || 'Scheduled Events' }}"",
        ""text"": ""={{ `Events for ${new Date().toLocaleDateString()}:\n\n` + $json.events.map(e => `- ${e.title} at ${e.time}`).join('\n') }}"",
        ""html"": ""={{ `<h2>Events for ${new Date().toLocaleDateString()}</h2><ul>` + $json.events.map(e => `<li><b>${e.title}</b> at ${e.time}</li>`).join('') + '</ul>' }}""
      },
      ""name"": ""Send Notification"",
      ""type"": ""n8n-nodes-base.emailSend"",
      ""position"": [850, 200]
    },
    {
      ""parameters"": {
        ""values"": {
          ""string"": [
            {
              ""name"": ""status"",
              ""value"": ""No events found for today""
            }
          ]
        }
      },
      ""name"": ""No Events"",
      ""type"": ""n8n-nodes-base.set"",
      ""position"": [850, 400]
    }
  ],
  ""connections"": {
    ""Schedule Trigger"": {
      ""main"": [
        [
          {
            ""node"": ""Fetch Events"",
            ""type"": ""main"",
            ""index"": 0
          }
        ]
      ]
    },
    ""Fetch Events"": {
      ""main"": [
        [
          {
            ""node"": ""Has Events"",
            ""type"": ""main"",
            ""index"": 0
          }
        ]
      ]
    },
    ""Has Events"": {
      ""main"": [
        [
          {
            ""node"": ""Send Notification"",
            ""type"": ""main"",
            ""index"": 0
          }
        ],
        [
          {
            ""node"": ""No Events"",
            ""type"": ""main"",
            ""index"": 0
          }
        ]
      ]
    }
  }
}",
                    Tags = new List<string> { "n8n", "Automation", "Workflow", "Notification", "JavaScript" },
                    CreatedDate = DateTime.Now.AddDays(-30),
                    UpdatedDate = DateTime.Now.AddDays(-5),
                    IsPublic = true,
                    ViewCount = 78
                },
                new CodeSnippet
                {
                    Id = 2,
                    Title = "Flutter Firebase Authentication",
                    Description = "Implementation of user authentication in Flutter using Firebase",
                    Language = "dart",
                    Code = @"// lib/services/auth_service.dart
import 'package:firebase_auth/firebase_auth.dart';
import 'package:cloud_firestore/cloud_firestore.dart';

class AuthService {
  final FirebaseAuth _auth = FirebaseAuth.instance;
  final FirebaseFirestore _firestore = FirebaseFirestore.instance;

  // Get current user
  User? get currentUser => _auth.currentUser;

  // Auth state changes stream
  Stream<User?> get authStateChanges => _auth.authStateChanges();

  // Sign in with email and password
  Future<UserCredential> signInWithEmailAndPassword(String email, String password) async {
    try {
      UserCredential result = await _auth.signInWithEmailAndPassword(
        email: email,
        password: password,
      );
      
      // Update last login timestamp
      await _firestore.collection('users').doc(result.user!.uid).update({
        'lastLogin': FieldValue.serverTimestamp(),
      });
      
      return result;
    } catch (e) {
      print('Error signing in: $e');
      rethrow;
    }
  }

  // Create user with email and password
  Future<UserCredential> createUserWithEmailAndPassword(
    String email, 
    String password, 
    String name,
    String role,
  ) async {
    try {
      UserCredential result = await _auth.createUserWithEmailAndPassword(
        email: email,
        password: password,
      );
      
      // Create user document in firestore
      await _firestore.collection('users').doc(result.user!.uid).set({
        'uid': result.user!.uid,
        'email': email,
        'name': name,
        'role': role,
        'createdAt': FieldValue.serverTimestamp(),
        'lastLogin': FieldValue.serverTimestamp(),
      });
      
      // Update user profile
      await result.user!.updateDisplayName(name);
      
      return result;
    } catch (e) {
      print('Error creating user: $e');
      rethrow;
    }
  }

  // Sign out
  Future<void> signOut() async {
    try {
      return await _auth.signOut();
    } catch (e) {
      print('Error signing out: $e');
      rethrow;
    }
  }

  // Reset password
  Future<void> resetPassword(String email) async {
    try {
      await _auth.sendPasswordResetEmail(email: email);
    } catch (e) {
      print('Error resetting password: $e');
      rethrow;
    }
  }

  // Update user profile
  Future<void> updateUserProfile(String displayName, String? photoURL) async {
    try {
      await _auth.currentUser!.updateDisplayName(displayName);
      
      if (photoURL != null) {
        await _auth.currentUser!.updatePhotoURL(photoURL);
      }
      
      await _firestore.collection('users').doc(_auth.currentUser!.uid).update({
        'name': displayName,
        'photoURL': photoURL,
        'updatedAt': FieldValue.serverTimestamp(),
      });
    } catch (e) {
      print('Error updating profile: $e');
      rethrow;
    }
  }

  // Delete user account
  Future<void> deleteAccount() async {
    try {
      String uid = _auth.currentUser!.uid;
      
      // Delete user document
      await _firestore.collection('users').doc(uid).delete();
      
      // Delete auth user
      await _auth.currentUser!.delete();
    } catch (e) {
      print('Error deleting account: $e');
      rethrow;
    }
  }
}",
                    Tags = new List<string> { "Flutter", "Dart", "Firebase", "Authentication", "Mobile" },
                    CreatedDate = DateTime.Now.AddDays(-45),
                    IsPublic = true,
                    ViewCount = 126
                },
                new CodeSnippet
                {
                    Id = 3,
                    Title = "AI ChatBot Prompt Engineering",
                    Description = "Techniques for effective prompt engineering with LLM models",
                    Language = "javascript",
                    Code = @"// chatbot-prompt-engineering.js
// Implementation for a LLM chatbot with context management

// Define the base system prompt to set chatbot behavior
const systemPrompt = `
You are an educational assistant specialized in programming and technology.
Your role is to:
1. Provide clear and accurate information about programming concepts
2. Explain complex topics in simple terms with examples
3. Help users learn to solve problems, not just provide solutions
4. Maintain a friendly, encouraging tone
5. Focus on being educational rather than just providing answers
`;

// Structure for maintaining conversation context
class ConversationContext {
  constructor(maxContextSize = 5) {
    this.messages = [];
    this.maxContextSize = maxContextSize;
  }

  // Add message to context
  addMessage(role, content) {
    this.messages.push({ role, content });
    
    // Trim context if exceeds maximum size
    if (this.messages.length > this.maxContextSize + 1) { // +1 to always keep system prompt
      // Remove oldest user/assistant messages but keep system prompt
      const systemMessage = this.messages[0].role === 'system' ? this.messages.shift() : null;
      this.messages.shift(); // Remove oldest message
      if (systemMessage) {
        this.messages.unshift(systemMessage); // Add system message back at the start
      }
    }
  }

  // Get full context including system prompt
  getFullContext() {
    // Ensure system prompt is at the beginning
    if (this.messages.length === 0 || this.messages[0].role !== 'system') {
      this.messages.unshift({ role: 'system', content: systemPrompt });
    }
    
    return this.messages;
  }
}

// Function to enhance user prompts with additional context
function enhanceUserPrompt(userPrompt, userProfile) {
  // Extract relevant skills and interests from user profile
  const relevantInterests = userProfile.interests
    .filter(interest => userPrompt.toLowerCase().includes(interest.toLowerCase()));
    
  // Add skill level context if detected
  let skillContext = '';
  if (userProfile.skillLevel && relevantInterests.length > 0) {
    skillContext = `The user has ${userProfile.skillLevel} knowledge in ${relevantInterests.join(', ')}.`;
  }
  
  // Construct enhanced prompt
  const enhancedPrompt = `
${userPrompt}

${skillContext ? `Context about the user: ${skillContext}` : ''}
`;

  return enhancedPrompt.trim();
}

// Function to generate response using LLM API
async function generateResponse(conversation, userPrompt, userProfile) {
  // Enhance the prompt based on user profile
  const enhancedPrompt = enhanceUserPrompt(userPrompt, userProfile);
  
  // Add user message to conversation
  conversation.addMessage('user', enhancedPrompt);
  
  try {
    // Get full conversation context
    const context = conversation.getFullContext();
    
    // Make API call to LLM service
    const response = await fetch('https://api.llm-service.com/chat', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer YOUR_API_KEY'
      },
      body: JSON.stringify({
        messages: context,
        temperature: 0.7,
        max_tokens: 500
      })
    });
    
    const data = await response.json();
    const botResponse = data.choices[0].message.content;
    
    // Add assistant response to conversation
    conversation.addMessage('assistant', botResponse);
    
    return botResponse;
  } catch (error) {
    console.error('Error generating response:', error);
    return 'I apologize, but I encountered an error processing your request.';
  }
}

// Example usage
const userProfile = {
  skillLevel: 'intermediate',
  interests: ['javascript', 'react', 'web development', 'machine learning']
};

const conversation = new ConversationContext();

// Initial system message
conversation.addMessage('system', systemPrompt);

// Handle user message
async function handleUserMessage(userMessage) {
  const response = await generateResponse(conversation, userMessage, userProfile);
  return response;
}

// Export functions for use in application
module.exports = {
  ConversationContext,
  handleUserMessage,
  enhanceUserPrompt
};",
                    Tags = new List<string> { "AI", "ChatBot", "LLM", "JavaScript", "Prompt Engineering" },
                    CreatedDate = DateTime.Now.AddDays(-10),
                    IsPublic = true,
                    ViewCount = 93
                },
                new CodeSnippet
                {
                    Id = 4,
                    Title = "WordPress Custom Rest API Endpoint",
                    Description = "Example of creating custom REST API endpoints in WordPress",
                    Language = "php",
                    Code = @"<?php
/**
 * Custom WordPress REST API Endpoints
 * 
 * Plugin Name: Custom API Endpoints
 * Description: Adds custom REST API endpoints for events and courses
 * Version: 1.0
 * Author: Panupol Sonnuam
 */

// Exit if accessed directly
if (!defined('ABSPATH')) {
    exit;
}

/**
 * Register custom REST API routes
 */
function custom_api_register_routes() {
    // Register events endpoint
    register_rest_route('custom/v1', '/events', array(
        'methods' => 'GET',
        'callback' => 'get_events_data',
        'permission_callback' => 'custom_api_permissions_check'
    ));
    
    // Register events by date endpoint
    register_rest_route('custom/v1', '/events/(?P<date>\d{4}-\d{2}-\d{2})', array(
        'methods' => 'GET',
        'callback' => 'get_events_by_date',
        'permission_callback' => 'custom_api_permissions_check',
        'args' => array(
            'date' => array(
                'validate_callback' => function($param) {
                    return preg_match('/^\d{4}-\d{2}-\d{2}$/', $param);
                }
            )
        )
    ));
    
    // Register courses endpoint
    register_rest_route('custom/v1', '/courses', array(
        'methods' => 'GET',
        'callback' => 'get_courses_data',
        'permission_callback' => 'custom_api_permissions_check'
    ));
    
    // Register course details endpoint
    register_rest_route('custom/v1', '/courses/(?P<id>\d+)', array(
        'methods' => 'GET',
        'callback' => 'get_course_details',
        'permission_callback' => 'custom_api_permissions_check',
        'args' => array(
            'id' => array(
                'validate_callback' => function($param) {
                    return is_numeric($param);
                }
            )
        )
    ));
    
    // Register course enrollment endpoint
    register_rest_route('custom/v1', '/courses/(?P<id>\d+)/enroll', array(
        'methods' => 'POST',
        'callback' => 'enroll_course',
        'permission_callback' => 'custom_api_authenticated_check',
        'args' => array(
            'id' => array(
                'validate_callback' => function($param) {
                    return is_numeric($param);
                }
            )
        )
    ));
}
add_action('rest_api_init', 'custom_api_register_routes');

/**
 * Check if user has permission to access endpoint
 */
function custom_api_permissions_check() {
    // Allow anyone to access this endpoint
    return true;
}

/**
 * Check if user is authenticated
 */
function custom_api_authenticated_check() {
    // Check if user is logged in
    return is_user_logged_in();
}

/**
 * Get all events
 */
function get_events_data($request) {
    $args = array(
        'post_type' => 'event',
        'posts_per_page' => -1,
        'post_status' => 'publish'
    );
    
    // Check for category parameter
    if (!empty($request['category'])) {
        $args['tax_query'] = array(
            array(
                'taxonomy' => 'event_category',
                'field' => 'slug',
                'terms' => $request['category']
            )
        );
    }
    
    $query = new WP_Query($args);
    $events = array();
    
    if ($query->have_posts()) {
        while ($query->have_posts()) {
            $query->the_post();
            $post_id = get_the_ID();
            
            $events[] = array(
                'id' => $post_id,
                'title' => get_the_title(),
                'date' => get_post_meta($post_id, 'event_date', true),
                'time' => get_post_meta($post_id, 'event_time', true),
                'location' => get_post_meta($post_id, 'event_location', true),
                'description' => get_the_excerpt(),
                'featured_image' => get_the_post_thumbnail_url($post_id, 'medium'),
                'permalink' => get_permalink()
            );
        }
        wp_reset_postdata();
    }
    
    return rest_ensure_response($events);
}

/**
 * Get events by date
 */
function get_events_by_date($request) {
    $date = sanitize_text_field($request['date']);
    
    $args = array(
        'post_type' => 'event',
        'posts_per_page' => -1,
        'post_status' => 'publish',
        'meta_query' => array(
            array(
                'key' => 'event_date',
                'value' => $date,
                'compare' => '='
            )
        )
    );
    
    $query = new WP_Query($args);
    $events = array();
    
    if ($query->have_posts()) {
        while ($query->have_posts()) {
            $query->the_post();
            $post_id = get_the_ID();
            
            $events[] = array(
                'id' => $post_id,
                'title' => get_the_title(),
                'time' => get_post_meta($post_id, 'event_time', true),
                'location' => get_post_meta($post_id, 'event_location', true),
                'description' => get_the_excerpt(),
                'featured_image' => get_the_post_thumbnail_url($post_id, 'medium'),
                'permalink' => get_permalink()
            );
        }
        wp_reset_postdata();
    }
    
    return rest_ensure_response(array(
        'date' => $date,
        'events' => $events
    ));
}

/**
 * Get all courses
 */
function get_courses_data($request) {
    $args = array(
        'post_type' => 'course',
        'posts_per_page' => -1,
        'post_status' => 'publish'
    );
    
    // Check for category parameter
    if (!empty($request['category'])) {
        $args['tax_query'] = array(
            array(
                'taxonomy' => 'course_category',
                'field' => 'slug',
                'terms' => $request['category']
            )
        );
    }
    
    $query = new WP_Query($args);
    $courses = array();
    
    if ($query->have_posts()) {
        while ($query->have_posts()) {
            $query->the_post();
            $post_id = get_the_ID();
            
            $courses[] = array(
                'id' => $post_id,
                'title' => get_the_title(),
                'description' => get_the_excerpt(),
                'featured_image' => get_the_post_thumbnail_url($post_id, 'medium'),
                'instructor' => get_post_meta($post_id, 'course_instructor', true),
                'duration' => get_post_meta($post_id, 'course_duration', true),
                'level' => get_post_meta($post_id, 'course_level', true),
                'price' => get_post_meta($post_id, 'course_price', true),
                'permalink' => get_permalink()
            );
        }
        wp_reset_postdata();
    }
    
    return rest_ensure_response($courses);
}

/**
 * Get course details
 */
function get_course_details($request) {
    $course_id = (int) $request['id'];
    
    // Check if course exists
    $course = get_post($course_id);
    
    if (!$course || $course->post_type !== 'course' || $course->post_status !== 'publish') {
        return new WP_Error('course_not_found', 'Course not found', array('status' => 404));
    }
    
    // Get course modules
    $modules = array();
    $module_args = array(
        'post_type' => 'module',
        'posts_per_page' => -1,
        'post_status' => 'publish',
        'meta_query' => array(
            array(
                'key' => 'module_course',
                'value' => $course_id,
                'compare' => '='
            )
        ),
        'orderby' => 'menu_order',
        'order' => 'ASC'
    );
    
    $module_query = new WP_Query($module_args);
    
    if ($module_query->have_posts()) {
        while ($module_query->have_posts()) {
            $module_query->the_post();
            $module_id = get_the_ID();
            
            $modules[] = array(
                'id' => $module_id,
                'title' => get_the_title(),
                'description' => get_the_excerpt(),
                'duration' => get_post_meta($module_id, 'module_duration', true)
            );
        }
        wp_reset_postdata();
    }
    
    // Build course details
    $course_details = array(
        'id' => $course_id,
        'title' => $course->post_title,
        'content' => apply_filters('the_content', $course->post_content),
        'featured_image' => get_the_post_thumbnail_url($course_id, 'large'),
        'instructor' => get_post_meta($course_id, 'course_instructor', true),
        'instructor_bio' => get_post_meta($course_id, 'course_instructor_bio', true),
        'duration' => get_post_meta($course_id, 'course_duration', true),
        'level' => get_post_meta($course_id, 'course_level', true),
        'price' => get_post_meta($course_id, 'course_price', true),
        'modules' => $modules,
        'permalink' => get_permalink($course_id)
    );
    
    return rest_ensure_response($course_details);
}

/**
 * Enroll user in course
 */
function enroll_course($request) {
    $course_id = (int) $request['id'];
    $user_id = get_current_user_id();
    
    // Check if course exists
    $course = get_post($course_id);
    
    if (!$course || $course->post_type !== 'course' || $course->post_status !== 'publish') {
        return new WP_Error('course_not_found', 'Course not found', array('status' => 404));
    }
    
    // Check if user is already enrolled
    $enrolled = get_user_meta($user_id, 'enrolled_courses', true);
    
    if (!$enrolled) {
        $enrolled = array();
    } else {
        $enrolled = maybe_unserialize($enrolled);
    }
    
    if (in_array($course_id, $enrolled)) {
        return new WP_Error('already_enrolled', 'User is already enrolled in this course', array('status' => 400));
    }
    
    // Add course to user's enrolled courses
    $enrolled[] = $course_id;
    update_user_meta($user_id, 'enrolled_courses', $enrolled);
    
    // Log enrollment
    $enrollment_date = current_time('mysql');
    add_user_meta($user_id, 'course_' . $course_id . '_enrolled', $enrollment_date);
    
    // Return success response
    return rest_ensure_response(array(
        'status' => 'success',
        'message' => 'Successfully enrolled in course',
        'course_id' => $course_id,
        'enrollment_date' => $enrollment_date
    ));
}",
                    Tags = new List<string> { "WordPress", "PHP", "REST API", "Web Development" },
                    CreatedDate = DateTime.Now.AddDays(-60),
                    UpdatedDate = DateTime.Now.AddDays(-15),
                    IsPublic = true,
                    ViewCount = 82
                },
                new CodeSnippet
                {
                    Id = 5,
                    Title = "Arduino Automatic Storage Control",
                    Description = "Arduino code for controlling motors in automated storage systems",
                    Language = "cpp",
                    Code = @"// ET-Table Project: Automatic Laptop Storage System
// Author: Panupol Sonnuam

#include <AccelStepper.h>
#include <Servo.h>
#include <EEPROM.h>

// Pin Definitions
#define MOTOR_STEP_PIN 3
#define MOTOR_DIR_PIN 4
#define MOTOR_ENABLE_PIN 5

#define SERVO_PIN 9

#define LIMIT_SWITCH_TOP 10
#define LIMIT_SWITCH_BOTTOM 11

#define BUTTON_OPEN 7
#define BUTTON_CLOSE 8
#define LED_STATUS 13

// Constants
#define MOTOR_MAX_SPEED 1000
#define MOTOR_ACCELERATION 500
#define STEPS_PER_REVOLUTION 200
#define MICROSTEPS 16
#define GEAR_RATIO 2 // 2:1 gear ratio

#define EEPROM_ADDR_CALIBRATED 0
#define EEPROM_ADDR_MAX_POS 1

// Create stepper motor object
AccelStepper stepper(AccelStepper::DRIVER, MOTOR_STEP_PIN, MOTOR_DIR_PIN);

// Create servo object
Servo lockServo;

// System state variables
enum SystemState {
  STATE_INIT,
  STATE_CALIBRATING,
  STATE_IDLE,
  STATE_OPENING,
  STATE_OPEN,
  STATE_CLOSING,
  STATE_CLOSED,
  STATE_ERROR
};

SystemState currentState = STATE_INIT;
SystemState previousState = STATE_INIT;

// Position tracking
long currentPosition = 0;
long maxPosition = 0;
bool isCalibrated = false;

// Button debounce
unsigned long lastDebounceTime = 0;
const unsigned long debounceDelay = 50;

// LED blinking
unsigned long lastBlinkTime = 0;
const unsigned long blinkInterval = 500;
bool ledState = false;

void setup() {
  // Initialize serial communication
  Serial.begin(9600);
  Serial.println(F(""ET-Table Automated Storage System""));
  Serial.println(F(""Version 1.0""));
  
  // Configure pins
  pinMode(MOTOR_ENABLE_PIN, OUTPUT);
  pinMode(LIMIT_SWITCH_TOP, INPUT_PULLUP);
  pinMode(LIMIT_SWITCH_BOTTOM, INPUT_PULLUP);
  pinMode(BUTTON_OPEN, INPUT_PULLUP);
  pinMode(BUTTON_CLOSE, INPUT_PULLUP);
  pinMode(LED_STATUS, OUTPUT);
  
  // Configure stepper motor
  stepper.setMaxSpeed(MOTOR_MAX_SPEED);
  stepper.setAcceleration(MOTOR_ACCELERATION);
  digitalWrite(MOTOR_ENABLE_PIN, HIGH); // Disable motor initially (active LOW)
  
  // Initialize servo
  lockServo.attach(SERVO_PIN);
  lockServo.write(0); // Unlock position
  
  // Check if system has been calibrated before
  if (EEPROM.read(EEPROM_ADDR_CALIBRATED) == 42) {
    isCalibrated = true;
    maxPosition = readLongFromEEPROM(EEPROM_ADDR_MAX_POS);
    Serial.print(F(""System is calibrated. Max position: ""));
    Serial.println(maxPosition);
    currentState = STATE_IDLE;
  } else {
    Serial.println(F(""System needs calibration""));
    currentState = STATE_CALIBRATING;
  }
}

void loop() {
  // State machine
  switch (currentState) {
    case STATE_INIT:
      // Should not reach here after setup
      currentState = STATE_CALIBRATING;
      break;
      
    case STATE_CALIBRATING:
      calibrateSystem();
      break;
      
    case STATE_IDLE:
      // Check buttons
      if (buttonPressed(BUTTON_OPEN) && !isLimitSwitchActive(LIMIT_SWITCH_TOP)) {
        currentState = STATE_OPENING;
        enableMotor();
        stepper.moveTo(maxPosition);
        Serial.println(F(""Opening storage compartment""));
      } else if (buttonPressed(BUTTON_CLOSE) && !isLimitSwitchActive(LIMIT_SWITCH_BOTTOM)) {
        currentState = STATE_CLOSING;
        enableMotor();
        stepper.moveTo(0);
        Serial.println(F(""Closing storage compartment""));
      }
      break;
      
    case STATE_OPENING:
      if (stepper.distanceToGo() == 0 || isLimitSwitchActive(LIMIT_SWITCH_TOP)) {
        if (isLimitSwitchActive(LIMIT_SWITCH_TOP)) {
          stepper.setCurrentPosition(maxPosition);
        }
        currentState = STATE_OPEN;
        disableMotor();
        Serial.println(F(""Storage compartment open""));
      }
      stepper.run();
      break;
      
    case STATE_OPEN:
      if (buttonPressed(BUTTON_CLOSE) && !isLimitSwitchActive(LIMIT_SWITCH_BOTTOM)) {
        currentState = STATE_CLOSING;
        enableMotor();
        stepper.moveTo(0);
        Serial.println(F(""Closing storage compartment""));
      }
      break;
      
    case STATE_CLOSING:
      if (stepper.distanceToGo() == 0 || isLimitSwitchActive(LIMIT_SWITCH_BOTTOM)) {
        if (isLimitSwitchActive(LIMIT_SWITCH_BOTTOM)) {
          stepper.setCurrentPosition(0);
        }
        currentState = STATE_CLOSED;
        disableMotor();
        Serial.println(F(""Storage compartment closed""));
      }
      stepper.run();
      break;
      
    case STATE_CLOSED:
      if (buttonPressed(BUTTON_OPEN) && !isLimitSwitchActive(LIMIT_SWITCH_TOP)) {
        currentState = STATE_OPENING;
        enableMotor();
        stepper.moveTo(maxPosition);
        Serial.println(F(""Opening storage compartment""));
      }
      break;
      
    case STATE_ERROR:
      // Blink LED rapidly to indicate error
      if (millis() - lastBlinkTime > 200) {
        lastBlinkTime = millis();
        ledState = !ledState;
        digitalWrite(LED_STATUS, ledState);
      }
      
      // Check if both buttons are pressed to reset
      if (!digitalRead(BUTTON_OPEN) && !digitalRead(BUTTON_CLOSE)) {
        delay(1000); // Hold for 1 second
        if (!digitalRead(BUTTON_OPEN) && !digitalRead(BUTTON_CLOSE)) {
          Serial.println(F(""Resetting system""));
          currentState = STATE_CALIBRATING;
        }
      }
      break;
  }
  
  // Update status LED based on state
  updateStatusLED();
  
  // Print state change for debugging
  if (currentState != previousState) {
    Serial.print(F(""State changed: ""));
    Serial.println(currentState);
    previousState = currentState;
  }
}

// System calibration procedure
void calibrateSystem() {
  Serial.println(F(""Starting calibration...""));
  blinkLED(3); // Visual indicator of calibration mode
  
  // Move to bottom position first (home)
  Serial.println(F(""Finding home position...""));
  enableMotor();
  
  // If bottom limit switch not active, move down
  if (!isLimitSwitchActive(LIMIT_SWITCH_BOTTOM)) {
    stepper.setMaxSpeed(MOTOR_MAX_SPEED / 2); // Slower for calibration
    stepper.setSpeed(-MOTOR_MAX_SPEED / 2);
    
    while (!isLimitSwitchActive(LIMIT_SWITCH_BOTTOM)) {
      stepper.runSpeed();
    }
  }
  
  // Set current position as zero
  delay(500); // Short delay for stability
  stepper.setCurrentPosition(0);
  Serial.println(F(""Home position set to 0""));
  
  // Now move to top position
  Serial.println(F(""Finding maximum position...""));
  stepper.setSpeed(MOTOR_MAX_SPEED / 2);
  
  while (!isLimitSwitchActive(LIMIT_SWITCH_TOP)) {
    stepper.runSpeed();
  }
  
  // Record the maximum position
  delay(500); // Short delay for stability
  maxPosition = stepper.currentPosition();
  Serial.print(F(""Maximum position: ""));
  Serial.println(maxPosition);
  
  // Save to EEPROM
  EEPROM.write(EEPROM_ADDR_CALIBRATED, 42); // Calibration flag
  writeLongToEEPROM(EEPROM_ADDR_MAX_POS, maxPosition);
  
  // Move back to home position
  stepper.setMaxSpeed(MOTOR_MAX_SPEED);
  stepper.setAcceleration(MOTOR_ACCELERATION);
  stepper.moveTo(0);
  
  while (stepper.distanceToGo() != 0) {
    stepper.run();
  }
  
  // Calibration complete
  isCalibrated = true;
  disableMotor();
  Serial.println(F(""Calibration complete""));
  currentState = STATE_IDLE;
}

// Utility functions
bool isLimitSwitchActive(int pin) {
  return !digitalRead(pin); // Limit switches are active LOW (normally open)
}

bool buttonPressed(int button) {
  static int lastButtonState[2] = {HIGH, HIGH}; // For OPEN and CLOSE buttons
  static unsigned long lastDebounceTime[2] = {0, 0};
  bool buttonEvent = false;
  int buttonIndex = (button == BUTTON_OPEN) ? 0 : 1;
  
  int reading = digitalRead(button);
  
  if (reading != lastButtonState[buttonIndex]) {
    lastDebounceTime[buttonIndex] = millis();
  }
  
  if ((millis() - lastDebounceTime[buttonIndex]) > debounceDelay) {
    if (reading == LOW) { // Button is pressed (active LOW)
      buttonEvent = true;
    }
  }
  
  lastButtonState[buttonIndex] = reading;
  return buttonEvent;
}

void enableMotor() {
  digitalWrite(MOTOR_ENABLE_PIN, LOW); // Enable motor (active LOW)
  delay(10); // Short delay for stability
}

void disableMotor() {
  digitalWrite(MOTOR_ENABLE_PIN, HIGH); // Disable motor (active LOW)
}

void updateStatusLED() {
  // Different LED patterns for different states
  switch (currentState) {
    case STATE_CALIBRATING:
      // Fast blinking during calibration
      if (millis() - lastBlinkTime > 200) {
        lastBlinkTime = millis();
        ledState = !ledState;
        digitalWrite(LED_STATUS, ledState);
      }
      break;
      
    case STATE_IDLE:
      // Slow breathing effect
      int brightness = (sin(millis() / 1000.0 * PI) + 1) * 127;
      analogWrite(LED_STATUS, brightness);
      break;
      
    case STATE_OPENING:
    case STATE_CLOSING:
      // Medium blink rate when moving
      if (millis() - lastBlinkTime > 300) {
        lastBlinkTime = millis();
        ledState = !ledState;
        digitalWrite(LED_STATUS, ledState);
      }
      break;
      
    case STATE_OPEN:
      // Solid on when open
      digitalWrite(LED_STATUS, HIGH);
      break;
      
    case STATE_CLOSED:
      // Off when closed
      digitalWrite(LED_STATUS, LOW);
      break;
  }
}

void blinkLED(int times) {
  for (int i = 0; i < times; i++) {
    digitalWrite(LED_STATUS, HIGH);
    delay(200);
    digitalWrite(LED_STATUS, LOW);
    delay(200);
  }
}

// EEPROM Utility functions for storing long values
void writeLongToEEPROM(int address, long value) {
  // Break long into 4 bytes
  byte byteArray[4];
  byteArray[0] = (value >> 24) & 0xFF;
  byteArray[1] = (value >> 16) & 0xFF;
  byteArray[2] = (value >> 8) & 0xFF;
  byteArray[3] = value & 0xFF;
  
  // Write the 4 bytes
  for (int i = 0; i < 4; i++) {
    EEPROM.write(address + i, byteArray[i]);
  }
}

long readLongFromEEPROM(int address) {
  // Read the 4 bytes
  byte byteArray[4];
  for (int i = 0; i < 4; i++) {
    byteArray[i] = EEPROM.read(address + i);
  }
  
  // Reconstruct the long
  return ((long)byteArray[0] << 24) | 
         ((long)byteArray[1] << 16) | 
         ((long)byteArray[2] << 8) | 
         (long)byteArray[3];
}",
                    Tags = new List<string> { "Arduino", "C++", "Hardware", "ET-Table", "Embedded Systems" },
                    CreatedDate = DateTime.Now.AddDays(-850),
                    UpdatedDate = DateTime.Now.AddDays(-700),
                    IsPublic = true,
                    ViewCount = 67
                }
            };
        }

        private List<Project> InitializeProjects()
        {
            return new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Title = "Automation System with n8n",
                    Description = "Comprehensive automation workflows using n8n low-code platform for scheduling, notifications, and database management.",
                    DetailedDescription = "Designed and developed automation systems for QUEST EDTECH that handled scheduling, notifications, and database management. Created workflows that automated class scheduling, student notifications, and integrated with various APIs. The system reduced manual administrative tasks by 60% and improved responsiveness to student inquiries. Implemented custom JavaScript functions within n8n to handle complex business logic and data transformations.",
                    ImageUrl = "/images/projects/automation.jpg",
                    GitHubUrl = "https://github.com/gotppsn/automation-workflows",
                    LiveDemoUrl = "",
                    Technologies = new List<string> { "JavaScript", "Python", "n8n", "APIs", "Webhooks" },
                    DateCompleted = new DateTime(2023, 8, 15),
                    Featured = true,
                    Category = "Automation",
                    Challenges = new List<string> { 
                        "Integrating with multiple third-party systems and APIs", 
                        "Creating reliable error handling and recovery mechanisms", 
                        "Building complex conditional workflows for different business scenarios"
                    },
                    Solutions = new List<string> { 
                        "Developed custom API integration nodes for specific services", 
                        "Implemented comprehensive logging and notification system for errors", 
                        "Created modular workflow components that could be reused across different processes" 
                    },
                    Screenshots = new List<string> {
                        "/images/projects/automation1.jpg",
                        "/images/projects/automation2.jpg",
                        "/images/projects/automation3.jpg"
                    }
                },
                new Project
                {
                    Id = 2,
                    Title = "WordPress Educational Websites",
                    Description = "Multi-lingual educational websites using WordPress with custom themes, plugins and integrations.",
                    DetailedDescription = "Led the development of multiple WordPress websites for QUEST EDTECH, creating over 100 pages across multiple languages (Thai, English, Chinese, Japanese). Implemented custom features for course registration, student portfolio display, and interactive learning materials. Optimized site performance and SEO for international audiences, with a strong focus on mobile responsiveness. Integrated the websites with payment systems and learning management systems.",
                    ImageUrl = "/images/projects/wordpress.jpg",
                    GitHubUrl = "",
                    LiveDemoUrl = "https://questlanguage.com",
                    Technologies = new List<string> { "WordPress", "PHP", "HTML/CSS", "JavaScript", "MySQL" },
                    DateCompleted = new DateTime(2022, 6, 30),
                    Featured = true,
                    Category = "Web Development",
                    Challenges = new List<string> { 
                        "Managing complex multilingual content across numerous pages", 
                        "Optimizing site performance while maintaining rich media content", 
                        "Creating intuitive registration and payment workflows"
                    },
                    Solutions = new List<string> { 
                        "Implemented WPML with custom translation management system", 
                        "Used advanced caching, CDN integration, and image optimization", 
                        "Developed custom plugins for streamlined registration process"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/wordpress1.jpg",
                        "/images/projects/wordpress2.jpg",
                        "/images/projects/wordpress3.jpg"
                    }
                },
                new Project
                {
                    Id = 3,
                    Title = "ProjectAppStock - Flutter App",
                    Description = "Mobile inventory management application built with Flutter and Firebase, featuring product tracking, barcode scanning, and sales management.",
                    DetailedDescription = "Developed a cross-platform mobile application for inventory management using Flutter and Firebase. The app provides real-time stock tracking, barcode scanning for quick product identification, sales reporting, and user role management. Features include inventory alerts, purchase order creation, sales analytics, and data export capabilities. This project was created as part of my university coursework, demonstrating my ability to work with modern mobile development frameworks.",
                    ImageUrl = "/images/projects/flutter.jpg",
                    GitHubUrl = "https://github.com/Gotppsn/ProjectAppStock-Flutter",
                    LiveDemoUrl = "",
                    Technologies = new List<string> { "Flutter", "Dart", "Firebase", "Firestore", "Authentication" },
                    DateCompleted = new DateTime(2023, 5, 20),
                    Featured = true,
                    Category = "Mobile",
                    Challenges = new List<string> { 
                        "Implementing real-time data synchronization across devices", 
                        "Creating an intuitive user interface for complex inventory operations", 
                        "Managing offline functionality and data synchronization"
                    },
                    Solutions = new List<string> { 
                        "Used Firebase Firestore for real-time data synchronization", 
                        "Designed custom UI components with Material Design principles", 
                        "Implemented local caching for offline access with synchronization"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/flutter1.jpg",
                        "/images/projects/flutter2.jpg",
                        "/images/projects/flutter3.jpg"
                    }
                },
                new Project
                {
                    Id = 4,
                    Title = "LLM-Powered AI Chatbots",
                    Description = "AI-powered chatbot solutions using Large Language Models for customer support and educational assistance.",
                    DetailedDescription = "Designed and implemented AI chatbot systems using Large Language Models (LLM) through the Flowise platform. Created specialized bots for answering frequently asked questions, guiding students through course materials, and providing personalized learning assistance. Used advanced prompt engineering techniques to ensure accurate and helpful responses. The chatbots were integrated with existing systems through APIs and webhooks to access contextual data.",
                    ImageUrl = "/images/projects/ai-chatbot.jpg",
                    GitHubUrl = "https://github.com/gotppsn/llm-chatbots",
                    LiveDemoUrl = "",
                    Technologies = new List<string> { "LLM", "Flowise", "JavaScript", "Node.js", "AI", "APIs" },
                    DateCompleted = new DateTime(2023, 11, 15),
                    Featured = false,
                    Category = "AI",
                    Challenges = new List<string> { 
                        "Fine-tuning language models for specialized educational domains", 
                        "Maintaining context in complex multi-turn conversations", 
                        "Optimizing response accuracy while managing computational resources"
                    },
                    Solutions = new List<string> { 
                        "Created domain-specific training datasets and prompt templates", 
                        "Implemented advanced context management with memory systems", 
                        "Used retrieval-augmented generation with knowledge bases"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/chatbot1.jpg",
                        "/images/projects/chatbot2.jpg"
                    }
                },
                new Project
                {
                    Id = 5,
                    Title = "Metaverse Classroom Environment",
                    Description = "Virtual learning environments created in Mozilla Hubs for immersive online education experiences.",
                    DetailedDescription = "Designed and developed virtual classroom environments in Mozilla Hubs for immersive educational experiences. Created interactive 3D spaces with educational resources, collaborative tools, and engaging activities. These environments were used for language classes and coding workshops, allowing students to interact with learning materials and each other in a virtual space. The project included custom 3D models, interactive elements, and spatial audio for natural communication.",
                    ImageUrl = "/images/projects/metaverse.jpg",
                    GitHubUrl = "",
                    LiveDemoUrl = "https://hubs.mozilla.com/example-classroom",
                    Technologies = new List<string> { "Mozilla Hubs", "3D Modeling", "JavaScript", "WebXR", "HTML/CSS" },
                    DateCompleted = new DateTime(2022, 9, 10),
                    Featured = false,
                    Category = "Education",
                    Challenges = new List<string> { 
                        "Creating accessible and intuitive 3D learning environments", 
                        "Optimizing performance across different devices and connections", 
                        "Designing effective educational interactions in virtual space"
                    },
                    Solutions = new List<string> { 
                        "Implemented accessibility features for diverse user needs", 
                        "Used level of detail techniques and asset optimization", 
                        "Created pedagogically-sound interactive learning elements"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/metaverse1.jpg",
                        "/images/projects/metaverse2.jpg"
                    }
                },
                new Project
                {
                    Id = 6,
                    Title = "ET-Table Project",
                    Description = "Automated laptop storage table using Arduino with motorized compartment for secure device storage.",
                    DetailedDescription = "Designed and built an innovative table with a motorized storage compartment for laptops using Arduino. Programmed the controller to manage motors, sensors, and user interface elements. The table featured automatic opening and closing of the storage compartment with safety mechanisms. This project won a gold medal in the software innovation and embedded systems category at a national competition, demonstrating the practical application of programming and electronics concepts.",
                    ImageUrl = "/images/projects/arduino.jpg",
                    GitHubUrl = "https://github.com/Gotppsn/Project_ET-Table",
                    LiveDemoUrl = "",
                    Technologies = new List<string> { "Arduino", "C++", "Electronics", "Embedded Systems", "3D Design" },
                    DateCompleted = new DateTime(2020, 3, 15),
                    Featured = false,
                    Category = "Hardware",
                    Challenges = new List<string> { 
                        "Integrating mechanical components with electronic controls", 
                        "Ensuring safety and reliability with moving parts", 
                        "Designing an intuitive user interface for operation"
                    },
                    Solutions = new List<string> { 
                        "Created custom PCB design for controller integration", 
                        "Implemented multiple safety features and emergency stops", 
                        "Designed simple button interface with status indicators"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/arduino1.jpg",
                        "/images/projects/arduino2.jpg"
                    }
                },
                new Project
                {
                    Id = 7,
                    Title = "AI Art Generation for Education",
                    Description = "Using AI image generation tools to create educational materials and visual content for learning.",
                    DetailedDescription = "Implemented AI art generation tools including Stable Diffusion and ComfyUI to create educational illustrations, story visuals, and marketing materials. Developed specialized prompt techniques to generate consistent character designs for educational content. Created workflows for generating series of related images for storytelling and educational sequences. This project streamlined content creation processes while maintaining visual consistency across materials.",
                    ImageUrl = "/images/projects/ai-art.jpg",
                    GitHubUrl = "",
                    LiveDemoUrl = "",
                    Technologies = new List<string> { "Stable Diffusion", "ComfyUI", "AI", "Image Generation", "Prompt Engineering" },
                    DateCompleted = new DateTime(2023, 12, 5),
                    Featured = false,
                    Category = "AI",
                    Challenges = new List<string> { 
                        "Generating consistent character designs across multiple images", 
                        "Creating culturally and age-appropriate educational imagery", 
                        "Optimizing image generation workflows for educational content"
                    },
                    Solutions = new List<string> { 
                        "Developed textual inversion and LoRA models for consistent characters", 
                        "Created comprehensive prompt libraries with educational guidelines", 
                        "Built automated batch processing workflows with metadata tagging"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/ai-art1.jpg",
                        "/images/projects/ai-art2.jpg"
                    }
                },
                new Project
                {
                    Id = 8,
                    Title = "Crypto Trading Signal Notifier",
                    Description = "Automated notification system for cryptocurrency trading signals using technical analysis and Line notifications.",
                    DetailedDescription = "Developed a personal project to automate cryptocurrency trading signals using technical analysis algorithms. The system monitors market data, identifies potential trading opportunities using configurable strategies, and sends notifications through Line Messaging API. Features include customizable technical indicators, risk management parameters, and real-time price alerts. While primarily a learning project, it provided valuable experience in financial algorithms and API integrations.",
                    ImageUrl = "/images/projects/crypto.jpg",
                    GitHubUrl = "https://github.com/gotppsn/crypto-signals",
                    LiveDemoUrl = "",
                    Technologies = new List<string> { "JavaScript", "Node.js", "MongoDB", "Technical Analysis", "Line API" },
                    DateCompleted = new DateTime(2022, 11, 20),
                    Featured = false,
                    Category = "Automation",
                    Challenges = new List<string> { 
                        "Developing reliable technical analysis indicators for trading signals", 
                        "Managing API rate limits and ensuring data accuracy", 
                        "Creating meaningful notifications without alert fatigue"
                    },
                    Solutions = new List<string> { 
                        "Implemented multiple indicator confirmations to reduce false signals", 
                        "Built data caching system with fallback data sources", 
                        "Created tiered notification system with importance levels"
                    },
                    Screenshots = new List<string> {
                        "/images/projects/crypto1.jpg"
                    }
                }
            };
        }

        private List<Experience> InitializeExperiences()
        {
            return new List<Experience>
            {
                new Experience
                {
                    Id = 1,
                    Company = "QUEST EDTECH COMPANY LIMITED",
                    Position = "Full-Stack Developer",
                    Description = "Led development of educational technology applications, automation systems, and AI implementations for this education technology startup.",
                    StartDate = new DateTime(2020, 5, 1),
                    EndDate = new DateTime(2024, 8, 31),
                    Location = "Bangkok, Thailand",
                    Achievements = new List<string>
                    {
                        "Developed automation systems using n8n platform, reducing administrative workload by 60%",
                        "Created multilingual WordPress websites with over 100 pages across Thai, English, Chinese, and Japanese",
                        "Implemented AI solutions including LLM chatbots and content generation tools",
                        "Built virtual classroom environments in Metaverse using MozillaHub",
                        "Designed and deployed database management systems for student records",
                        "Taught coding courses and managed intern teams",
                        "Integrated payment processing systems with educational platforms",
                        "Implemented SEO optimizations resulting in 45% traffic increase"
                    },
                    CompanyLogoUrl = "/images/companies/quest-logo.png",
                    CompanyWebsite = "https://questlanguage.com",
                    Technologies = new List<string> { "JavaScript", "Python", "WordPress", "n8n", "AI", "Metaverse", "HTML/CSS", "PHP" }
                },
                new Experience
                {
                    Id = 2,
                    Company = "Green Technology Engineering Co. Ltd",
                    Position = "Intern",
                    Description = "Completed a 3-month internship program gaining hands-on experience in business operations and technical support.",
                    StartDate = new DateTime(2019, 3, 1),
                    EndDate = new DateTime(2019, 5, 31),
                    Location = "Bangkok, Thailand",
                    Achievements = new List<string>
                    {
                        "Designed graphics for marketing materials and company documentation",
                        "Assisted in customer meetings with supervisors for technical projects",
                        "Helped with booth setup and presentation at INFOCOMM ASIA 2019 event",
                        "Supported general administrative tasks and document preparation",
                        "Participated in technical team meetings and project planning sessions"
                    },
                    CompanyLogoUrl = "/images/companies/green-tech-logo.png",
                    CompanyWebsite = "https://greentechengineering.co.th",
                    Technologies = new List<string> { "Graphic Design", "Marketing", "Customer Relations", "Technical Support" }
                },
                new Experience
                {
                    Id = 3,
                    Company = "Freelance Coding Tutor",
                    Position = "Programming Instructor",
                    Description = "Provided personalized programming instruction to students preparing for university entrance exams and professional developers looking to expand their skills.",
                    StartDate = new DateTime(2021, 6, 1),
                    EndDate = null, // Current position
                    Location = "Remote",
                    Achievements = new List<string>
                    {
                        "Developed custom curriculum for full-stack web development",
                        "Helped over 20 students prepare for university entrance interviews",
                        "Created interactive coding exercises and projects for hands-on learning",
                        "Specialized in JavaScript, Python, and web development technologies",
                        "Maintained 90% success rate for students entering technical programs"
                    },
                    CompanyLogoUrl = "/images/companies/tutor-logo.png",
                    CompanyWebsite = "",
                    Technologies = new List<string> { "JavaScript", "Python", "Web Development", "Teaching", "Curriculum Development" }
                }
            };
        }

        private List<Skill> InitializeSkills()
        {
            return new List<Skill>
            {
                // Programming Languages
                new Skill { Id = 1, Name = "Python", ProficiencyLevel = 5, Category = "Programming Languages", IconUrl = "/images/skills/python.png", Description = "Advanced knowledge with experience in automation, data processing, and API development" },
                new Skill { Id = 2, Name = "JavaScript", ProficiencyLevel = 5, Category = "Programming Languages", IconUrl = "/images/skills/javascript.png", Description = "Advanced proficiency with extensive use in web development and automation workflows" },
                new Skill { Id = 3, Name = "C++", ProficiencyLevel = 3, Category = "Programming Languages", IconUrl = "/images/skills/cpp.png", Description = "Intermediate skills used in embedded systems projects" },
                new Skill { Id = 4, Name = "Dart", ProficiencyLevel = 3, Category = "Programming Languages", IconUrl = "/images/skills/dart.png", Description = "Intermediate knowledge for Flutter mobile app development" },
                new Skill { Id = 5, Name = "PHP", ProficiencyLevel = 2, Category = "Programming Languages", IconUrl = "/images/skills/php.png", Description = "Basic knowledge used in WordPress theme customization" },
                new Skill { Id = 6, Name = "C#", ProficiencyLevel = 2, Category = "Programming Languages", IconUrl = "/images/skills/csharp.png", Description = "Basic knowledge with some experience in small projects" },
                
                // Frameworks & Tools
                new Skill { Id = 7, Name = "n8n", ProficiencyLevel = 5, Category = "Frameworks & Tools", IconUrl = "/images/skills/n8n.png", Description = "Expert in building complex automation workflows and integrations" },
                new Skill { Id = 8, Name = "WordPress", ProficiencyLevel = 5, Category = "Frameworks & Tools", IconUrl = "/images/skills/wordpress.png", Description = "Advanced skills in theme customization, plugin development, and multilingual sites" },
                new Skill { Id = 9, Name = "Flutter", ProficiencyLevel = 3, Category = "Frameworks & Tools", IconUrl = "/images/skills/flutter.png", Description = "Intermediate knowledge for cross-platform mobile application development" },
                new Skill { Id = 10, Name = "Laravel", ProficiencyLevel = 2, Category = "Frameworks & Tools", IconUrl = "/images/skills/laravel.png", Description = "Basic experience with PHP framework for web applications" },
                new Skill { Id = 11, Name = "Angular", ProficiencyLevel = 3, Category = "Frameworks & Tools", IconUrl = "/images/skills/angular.png", Description = "Working knowledge for frontend web application development" },
                new Skill { Id = 12, Name = "Node.js", ProficiencyLevel = 4, Category = "Frameworks & Tools", IconUrl = "/images/skills/nodejs.png", Description = "Strong proficiency in server-side JavaScript applications" },
                new Skill { Id = 13, Name = "Git", ProficiencyLevel = 4, Category = "Frameworks & Tools", IconUrl = "/images/skills/git.png", Description = "Proficient in version control and collaborative development workflows" },
                
                // Web Technologies
                new Skill { Id = 14, Name = "HTML/CSS", ProficiencyLevel = 4, Category = "Web Technologies", IconUrl = "/images/skills/htmlcss.png", Description = "Strong knowledge in creating responsive and accessible web interfaces" },
                new Skill { Id = 15, Name = "Bootstrap", ProficiencyLevel = 4, Category = "Web Technologies", IconUrl = "/images/skills/bootstrap.png", Description = "Extensive experience with responsive design framework" },
                new Skill { Id = 16, Name = "REST APIs", ProficiencyLevel = 4, Category = "Web Technologies", IconUrl = "/images/skills/api.png", Description = "Strong skills in designing and implementing RESTful services" },
                new Skill { Id = 17, Name = "SEO", ProficiencyLevel = 3, Category = "Web Technologies", IconUrl = "/images/skills/seo.png", Description = "Experience optimizing websites for search engines and performance" },
                
                // Databases
                new Skill { Id = 18, Name = "MySQL", ProficiencyLevel = 4, Category = "Databases", IconUrl = "/images/skills/mysql.png", Description = "Strong knowledge of relational database design and optimization" },
                new Skill { Id = 19, Name = "Firebase", ProficiencyLevel = 4, Category = "Databases", IconUrl = "/images/skills/firebase.png", Description = "Proficient in using Firebase for mobile and web applications" },
                new Skill { Id = 20, Name = "MongoDB", ProficiencyLevel = 3, Category = "Databases", IconUrl = "/images/skills/mongodb.png", Description = "Working knowledge of NoSQL database for document storage" },
                
                // AI & Machine Learning
                new Skill { Id = 21, Name = "LLM Integration", ProficiencyLevel = 4, Category = "AI & Machine Learning", IconUrl = "/images/skills/llm.png", Description = "Strong experience implementing large language models in applications" },
                new Skill { Id = 22, Name = "Prompt Engineering", ProficiencyLevel = 4, Category = "AI & Machine Learning", IconUrl = "/images/skills/prompt.png", Description = "Advanced techniques for effective AI prompt creation and optimization" },
                new Skill { Id = 23, Name = "Image Generation", ProficiencyLevel = 3, Category = "AI & Machine Learning", IconUrl = "/images/skills/image-gen.png", Description = "Experience with Stable Diffusion and ComfyUI for AI image creation" },
                
                // Education Technology
                new Skill { Id = 24, Name = "Metaverse Development", ProficiencyLevel = 4, Category = "Education Technology", IconUrl = "/images/skills/metaverse.png", Description = "Creating virtual educational environments in Mozilla Hubs" },
                new Skill { Id = 25, Name = "Curriculum Design", ProficiencyLevel = 3, Category = "Education Technology", IconUrl = "/images/skills/curriculum.png", Description = "Experience designing programming courses and learning materials" },
                new Skill { Id = 26, Name = "Educational Content", ProficiencyLevel = 4, Category = "Education Technology", IconUrl = "/images/skills/content.png", Description = "Creating effective digital materials for technical education" }
            };
        }

        private List<BlogPost> InitializeBlogPosts()
        {
            return new List<BlogPost>
            {
                new BlogPost
                {
                    Id = 1,
                    Title = "Building Efficient Automation Systems with n8n Platform",
                    Excerpt = "How I leveraged the n8n low-code platform to create powerful automation workflows for business processes, including notification systems, scheduling, and database management.",
                    Content = @"# Building Efficient Automation Systems with n8n Platform

Over the past three years at QUEST EDTECH, I've had the opportunity to implement numerous automation systems using the n8n low-code platform. These systems have dramatically improved our operational efficiency by automating repetitive tasks, ensuring timely notifications, and streamlining database management.

## What is n8n?

For those unfamiliar, n8n is an open-source, self-hostable workflow automation tool that allows you to connect different systems and automate tasks without writing extensive code. The platform uses a node-based visual interface where each node represents a specific action or integration.

## Key Automation Systems Implemented

### Class Scheduling Automation

One of the most impactful systems I developed was an automated class scheduling workflow. This system:

- Monitors our scheduling database for new class bookings
- Verifies teacher availability and classroom resources
- Creates Google Calendar events for all participants
- Sends confirmation emails with calendar invites
- Updates our internal systems with the confirmed schedule

This automation reduced scheduling errors by 95% and saved our administrative team approximately 15 hours per week.

### Student Notification System

Another crucial automation was our student notification system. This multi-channel system:

- Sends reminders 24 hours before scheduled classes
- Delivers personalized study materials before each session
- Notifies students of assignment deadlines
- Provides progress updates and achievement notifications
- Requests feedback after class completion

The implementation increased student attendance by 22% and significantly improved engagement with learning materials.

### Database Management Workflows

Managing student and course data across multiple systems was a significant challenge. I created workflows that:

- Synchronize data between our CRM, LMS, and payment systems
- Perform nightly database backups with verification
- Clean and standardize incoming data from various sources
- Generate reports for different departments
- Monitor database performance and issue alerts

## Technical Implementation Highlights

### Custom JavaScript Functions

While n8n provides many built-in operations, custom JavaScript functions were essential for complex business logic:

```javascript
// Example of a custom data transformation function
function transformStudentData(items) {
  return items.map(item => {
    // Standardize name format
    const nameParts = item.json.fullName.split(' ');
    const firstName = nameParts[0];
    const lastName = nameParts.slice(1).join(' ');
    
    // Calculate student level based on test scores
    let level = 'Beginner';
    if (item.json.testScore > 80) {
      level = 'Advanced';
    } else if (item.json.testScore > 50) {
      level = 'Intermediate';
    }
    
    // Format phone number consistently
    let phone = item.json.phone.replace(/\D/g, '');
    if (phone.startsWith('0')) {
      phone = '+66' + phone.substring(1);
    }
    
    return {
      ...item.json,
      firstName,
      lastName,
      level,
      phone,
      lastUpdated: new Date().toISOString()
    };
  });